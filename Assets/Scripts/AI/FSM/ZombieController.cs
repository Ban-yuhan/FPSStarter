using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour, IDamageable
{
    public float viewDistance = 15.0f; //시야 거리(시야에 들어오는 거리)

    public float viewAngle = 60.0f; //시야각

    public float hearingDistance = 8.0f; //청각 거리(소리를 들을 수 있는 거리)

    public LayerMask obstacleMask; //장애물 레이어 마스크

    //[SerializeField]
    //private float patrolRadius = 10.0f; //순찰 반경

    public Transform[] wayPoints; //웨이포인트의 배열


    public int currentWaypointIndex = 0; //현재 목표 지점의 인덱스


    public Transform targetPlayer; //플레이어의 transform정보


    public FPSMovement playerMovement; //플레이어 이동 스크립트를 담을 변수


    public NavMeshAgent agent;

    public Animator animator;

    public float attackRange = 1.5f; //공격 가능 사거리

    public float attackRate = 1.0f; //공격 속도(초당 공격 횟수)

    public float AttackDamage = 10.0f; //공격 데미지

    public RagdollController ragdoll;

    public float maxHealth = 100.0f;


    public float currentHealth;


    public float LastAttackTime = 0.0f; //마지막 공격 시점


    public float idleTimer = 0.0f;
    public float idleDuration = 2.0f; //2초 동안 대기 후 이동

    private ZombieState currentState;


    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        if (go != null)
        {
            targetPlayer = go.transform;
            playerMovement = go.GetComponent<FPSMovement>();
        }

        currentHealth = maxHealth;

        ChangeState(new IdleState(this));
        Debug.Log("현재상태 : " + currentState);

    }


    private void Update()
    {
        if(currentState == null)
        {
            return;
        }

        currentState.Update();
    }


    public void ChangeState(ZombieState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter();
       Debug.Log("현재상태 : " + currentState);
    }


    /// <summary>
    /// 애니메이션 이벤트 함수.
    /// 플레이어 캐릭터에 정해진 프레임에 데미지 적용
    /// </summary>
    public void TakeDamage()
    {
        IDamageable playerHealth = targetPlayer.GetComponent<IDamageable>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(AttackDamage);
        }
    }


    public void TakeDamage(float damageAmount)
    {
        if (currentState is DeadState)
        {
            return;
        }

        currentHealth -= damageAmount;

        if (!(currentState is ChaseState) && !(currentState is AttackState))
        {
            ChangeState(new ChaseState(this));
        }

        if (currentHealth <= 0.0f)
        {
            ChangeState(new DeadState(this));
        }
    }


    public bool DetectPlayer(float distance)
    {
       
            //청각 감지(거리 + 플레이어 이동 여부) -> 등 뒤에 있어도 가깝고, 플레이어가 움직이면 감지.
        if (distance <= hearingDistance)
        {
            if (playerMovement != null && playerMovement.IsMoving() == true)
            {
                return true;
            }
        }
        

        //시각 감지 (거리 + 시야각 + 장애물 여부)
        if (distance <= viewDistance)
        {
            Vector3 dirToTarget = (targetPlayer.position - transform.position).normalized;

            //자신의 정면과 타겟 방향 사이의 각도.
            float angle = Vector3.Angle(transform.forward, dirToTarget); //Vector3.Angle : 두 벡터 사이의 각도를 반환

            //각도가 시야각의 절반 이내인지 체크
            if (angle < viewAngle * 0.5f)
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
}
