using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private Transform targetPlayer;

    [SerializeField]
    private float pathUpdateDelay = 0.2f; //경로 갱신 딜레이 시간. 이게 너무 짧으면 끊기는 현상이 발생할 수 있음.

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator; //이동시 달리는 모션 출력을 위한 애니메이터 변수

    [SerializeField]
    private float stopChasingDistance = 20.0f; //추적을 멈추는 거리


    private float nextUpdateTime = 0.0f;


    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); 
        if(player != null)
        {
            targetPlayer = player.transform;
        }
    }


    private void Update()
    {
        if(targetPlayer == null)
        {
            return;
        }   
        
        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        if (distance < stopChasingDistance)
        {
            if (Time.time >= nextUpdateTime)
            {
                nextUpdateTime = Time.time + pathUpdateDelay;

                agent.SetDestination(targetPlayer.position);
            }
        }
        else
        {
            agent.ResetPath();
        }

        bool isMoving = agent.velocity.sqrMagnitude > 0.1f;

        if(animator != null)
        {
            animator.SetBool("Move", isMoving);
        }
    }
}
