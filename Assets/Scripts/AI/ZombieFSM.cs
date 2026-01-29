using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle = 0,   //대기
    Patrol = 1, //순찰
    Chase = 2,  //추적
}

public class ZombieFSM : MonoBehaviour
{
    [SerializeField]
    private EnemyState currentState; //현재상태 - 인스펙터에서 현재상태를 확인하기 위해 선언

    [SerializeField]
    private float detectionRange = 10.0f; //감지 거리 (범위 내에 들어오면 추적 상태로 전이)

    [SerializeField]
    private float patrolRadius = 10.0f; //순찰 반경

    [SerializeField]
    private float CanAttackRange = 3.0f; //공격 가능 거리

    [SerializeField]
    private Transform targetPlayer; //플레이어의 transform정보

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    private float idleTimer = 0.0f;
    private float idleDuration = 2.0f; //2초 동안 대기 후 이동


    private void Start()
    {
        currentState = EnemyState.Idle;

        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if(go != null)
        {
            targetPlayer = go.transform;
        }
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
                    SetRandomPatrolPoint();
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

        if (agent.pathPending == false && agent.remainingDistance < 0.5f)
        //agent.pathPending : 현재 경로를 계산중인지 여부. -> 계산 중 : true, 계산 중이 아님 : false
        //agent.remaningDistance : 남은 거리
        {
            //도착했으면 다시 대기 상태로 전이.
            ChangeState(EnemyState.Idle);
        }
    }


    void UpdateChase()
    {
        agent.SetDestination(targetPlayer.position);
    }


    /// <summary>
    /// 랜덤한 순찰 지점을 찾는다.
    /// </summary>
    void SetRandomPatrolPoint()
    {
        //내 위치를 기준으로 순찰 반경 안의 랜덤 좌표를 생성.
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius; //Random.insideUnitSphere : 반지름이 1인 구 생성. 그 구 안에서 랜덤하게 하나의 점 반환
        randomDirection += transform.position;

        NavMeshHit hit;

        //생성한 랜덤 좌표가 NavMesh위의 유효한 좌표인지 체크.
        if(NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1) == true)
        {
            agent.SetDestination(hit.position);
        }
    }


    void CheckTransitions()
    {
        if(targetPlayer == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, targetPlayer.position);

        //플레이어가 감지 범위 내에 들어오면(+이미 추적상태인게 아니라면) 추적 상태로 전이.
        if (distanceToPlayer <= detectionRange && distanceToPlayer > CanAttackRange && currentState != EnemyState.Chase)
        {
            ChangeState(EnemyState.Chase);
        }

        //플레이어가 감지 거리 바깥으로 멀어졌고, 현재 상태가 추적 상태라면 순찰 상태로 전이
        else if(distanceToPlayer > detectionRange && currentState == EnemyState.Chase)
        {
            ChangeState(EnemyState.Patrol);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); //감지 거리 시각화 (중심위치, 반지름)
    }
}
