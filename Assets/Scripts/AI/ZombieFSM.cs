using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle = 0,   //대기
    Patrol = 1, //순찰
    Chase = 2,  //추적
    Attack = 3, //공격
    Dead = 4,   //사망
}

public class ZombieFSM : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyState currentState; //현재상태 - 인스펙터에서 현재상태를 확인하기 위해 선언

    //[SerializeField]
    //private float detectionRange = 10.0f; //감지 거리 (범위 내에 들어오면 추적 상태로 전이)

    [SerializeField]
    private float viewDistance = 15.0f; //시야 거리(시야에 들어오는 거리)

    [SerializeField]
    private float viewAngle = 60.0f; //시야각

    [SerializeField]
    private float hearingDistance = 8.0f; //청각 거리(소리를 들을 수 있는 거리)

    [SerializeField]
    private LayerMask obstacleMask; //장애물 레이어 마스크

    //[SerializeField]
    //private float patrolRadius = 10.0f; //순찰 반경

    [SerializeField]
    private Transform[] wayPoints; //웨이포인트의 배열


    private int currentWaypointIndex = 0; //현재 목표 지점의 인덱스


    [SerializeField]
    private Transform targetPlayer; //플레이어의 transform정보

    
    private FPSMovement playerMovement; //플레이어 이동 스크립트를 담을 변수


    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float attackRange = 1.5f; //공격 가능 사거리

    [SerializeField]
    private float attackRate = 1.0f; //공격 속도(초당 공격 횟수)

    [SerializeField]
    private float AttackDamaage = 10.0f; //공격 데미지

    [SerializeField]
    private RagdollController ragdoll;

    [SerializeField]
    private float maxHealth = 100.0f;
    

    private float currentHealth;


    private float LastAttackTime = 0.0f; //마지막 공격 시점


    private float idleTimer = 0.0f;
    private float idleDuration = 2.0f; //2초 동안 대기 후 이동

    private bool HearingSound = false;
    private float hearingIdleTimer = 0.0f;
    private float hearingIdleDuration = 3.0f; //소리를 들은 후 5초 동안은 소리에 반응


    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            targetPlayer = go.transform;
            playerMovement = go.GetComponent<FPSMovement>();
        }

        currentHealth = maxHealth;

        currentState = EnemyState.Idle;
    }


    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                {
                    UpdateIdle();
                }
                break; 
            
            case EnemyState.Patrol:
                {
                    UpdatePatrol();
                }
                break;
            
            case EnemyState.Chase:
                {
                    UpdateChase();
                }
                break;

            case EnemyState.Attack:
                {
                    UpdateAttack();
                }
                break;
        }

        //상태 전이 체크
        CheckTransitions();
    }


    /// <summary>
    /// 상태 변경
    /// </summary>
    /// <param name="newState">변경할 상태</param>
    void ChangeState(EnemyState newState)
    {
        if(currentState == newState)
        {
            return;
        }

        currentState = newState;

        switch (currentState)
        {
            case EnemyState.Idle:
                {
                    idleTimer = 0.0f;
                    agent.ResetPath();
                    animator.SetBool("Move", false);
                }
                break;
            
            case EnemyState.Patrol:
                {
                    //SetRandomPatrolPoint();
                    if(wayPoints.Length > 0)
                    {
                        agent.SetDestination(wayPoints[currentWaypointIndex].position);
                    }

                    animator.SetBool("Move", true);
                }
                break;

            case EnemyState.Chase:
                {
                    animator.SetBool("Move", true);
                }
                break;
        }
    }


    void UpdateIdle()
    {
        idleTimer += Time.deltaTime;

        if(idleTimer >= idleDuration)
        {
            ChangeState(EnemyState.Patrol);
        }
    }


    void UpdatePatrol()
    {
        if(wayPoints.Length == 0)
        {
            return;
        }

        if (agent.pathPending == false && agent.remainingDistance < 0.5f)
        //agent.pathPending : 현재 경로를 계산중인지 여부. -> 계산 중 : true, 계산 중이 아님 : false
        //agent.remaningDistance : 남은 거리
        {
            //웨이포인트의 순서를 다음 순서로 갱신(마지막 웨이포인트에 도달했으면 처음 웨이포인트로 다시 돌아감)
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;

            //도착했으면 다시 대기 상태로 전이.
            ChangeState(EnemyState.Idle);
        }
    }


    void UpdateChase()
    {
        if (targetPlayer != null)
        {
            if (agent.enabled == true)
            {
                agent.SetDestination(targetPlayer.position);
                agent.isStopped = false; //추적 시 이동 재개.
            }
        }
    }

    void UpdateAttack()
    {
        agent.isStopped = true; //이동 멈춤

        if(targetPlayer != null)
        {
            Vector3 targetPosition = new Vector3(targetPlayer.position.x, transform.position.y, targetPlayer.position.z);
            transform.LookAt(targetPosition); //transform.LookAt : 파라미터로 전달한 위치를 바라보게 만들어주는 transform이 제공하는 함수.

            //공격 주기 체크
            if(Time.time >= LastAttackTime + attackRate)
            {
                LastAttackTime = Time.time;

                animator.SetTrigger("Attack");

                IDamageable playerHealth = targetPlayer.GetComponent<IDamageable>();

                if(playerHealth != null)
                {
                    playerHealth.TakeDamage(AttackDamaage);
                }
            }
        }
    }


    /// <summary>
    /// 랜덤한 순찰 지점을 찾는다.
    /// </summary>
    //void SetRandomPatrolPoint()
    //{
    //    //내 위치를 기준으로 순찰 반경 안의 랜덤 좌표를 생성.
    //    Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; //Random.insideUnitSphere : 반지름이 1인 구 생성. 그 구 안에서 랜덤하게 하나의 점 반환
    //    randomDirection += transform.position;

    //    NavMeshHit hit;

    //    //생성한 랜덤 좌표가 NavMesh위의 유효한 좌표인지 체크.
    //    if(NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1) == true)
    //    {
    //        agent.SetDestination(hit.position);
    //    }
    //}


    void CheckTransitions()
    {
        if(targetPlayer == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

        ////플레이어가 감지 범위 내에 들어오면(+이미 추적상태인게 아니라면) 추적 상태로 전이.
        //if (distanceToPlayer <= detectionRange && distanceToPlayer > CanAttackRange && currentState != EnemyState.Chase)
        //{
        //    ChangeState(EnemyState.Chase);
        //}

        ////플레이어가 감지 거리 바깥으로 멀어졌고, 현재 상태가 추적 상태라면 순찰 상태로 전이
        //else if(distanceToPlayer > detectionRange && currentState == EnemyState.Chase)
        //{
        //    ChangeState(EnemyState.Patrol);
        //}

        if(currentState == EnemyState.Chase)
        {
            if(distanceToPlayer <= attackRange)
            {
                ChangeState(EnemyState.Attack);
            }
            else if(distanceToPlayer > viewDistance)
            {
                ChangeState(EnemyState.Patrol);
            }
        }
        else if(currentState == EnemyState.Attack)
        {
            if(distanceToPlayer > attackRange)
            {
                ChangeState(EnemyState.Chase);
            }
        }
        else
        {
            if (DetectPlayer(distanceToPlayer) == true)
            {
                if (HearingSound == true)
                {
                    agent.SetDestination(targetPlayer.position);
                    hearingIdleTimer += Time.deltaTime;

                    if (hearingIdleTimer >= hearingIdleDuration)
                    {
                        HearingSound = false;
                        hearingIdleTimer = 0.0f;

                        ChangeState(EnemyState.Patrol);
                    }
                }
                else
                {
                    ChangeState(EnemyState.Chase);
                }
            }
        }
    }


    /// <summary>
    /// 시각 및 청각 감지 여부를 판단.
    /// </summary>
    /// <param name="distance"></param>
    bool DetectPlayer(float distance)
    {
        if (currentState == EnemyState.Patrol || currentState == EnemyState.Idle)
        {
            //청각 감지(거리 + 플레이어 이동 여부) -> 등 뒤에 있어도 가깝고, 플레이어가 움직이면 감지.
            if (distance <= hearingDistance)
            {
                if (playerMovement != null && playerMovement.IsMoving() == true)
                {
                    HearingSound = true;
                    Debug.Log("소리 감지");

                    return true;
                }
            }
        }

        //시각 감지 (거리 + 시야각 + 장애물 여부)
        if(distance <= viewDistance)
        {
            Vector3 dirToTarget = (targetPlayer.position - transform.position).normalized;

            //자신의 정면과 타겟 방향 사이의 각도.
            float angle = Vector3.Angle(transform.forward, dirToTarget); //Vector3.Angle : 두 벡터 사이의 각도를 반환

            //각도가 시야각의 절반 이내인지 체크
            if(angle < viewAngle * 0.5f)
            {
                //장애물 체크
                if (Physics.Raycast(transform.position + Vector3.up, dirToTarget, distance, obstacleMask) == false) //transform.position + Vector3.up : 오브젝트의 눈높이에서 레이캐스트 발사
                {
                    return true;
                }
            }
        }

        return false;
    }


    public void TakeDamage(float damageAmount)
    {
        if(currentState == EnemyState.Dead)
        {
            return;
        }

        currentHealth -= damageAmount;

        if(currentState != EnemyState.Chase && currentState!= EnemyState.Attack)
        {
            ChangeState(EnemyState.Chase);
        }

        if(currentHealth <= 0.0f)
        {
            Die();
        }
    }


    void Die()
    {
        ChangeState(EnemyState.Dead);
        agent.isStopped = true; //이동 멈춤
        agent.enabled = false; //NavMeshAgent 비활성화

        GetComponent<Collider>().enabled = false; //콜라이더 비활성화

        if(ragdoll != null)
        {
            ragdoll.EnableRagdoll();
        }

        Destroy(gameObject, 5.0f); //5초 후 오브젝트 삭제
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance); //감지 거리 시각화 (중심위치, 반지름)

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hearingDistance);//청각 거리 시각화

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange); 
    }
}
