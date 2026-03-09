using UnityEngine;

public class PatrolState : ZombieState
{
    public PatrolState(ZombieController zombieController) : base(zombieController)
    {

    }


    public override void Enter()
    {
        if(zombie.wayPoints.Length > 0)
        {
            zombie.agent.SetDestination(zombie.wayPoints[zombie.currentWaypointIndex].position);
        }

        zombie.animator.SetBool("Move", true);
    }


    public override void Update()
    {
        //감지 체크
        if (zombie.targetPlayer != null)
        {
            float dist = Vector3.Distance(zombie.transform.position, zombie.targetPlayer.transform.position);

            if (zombie.DetectPlayer(dist) == true)
            {
                zombie.ChangeState(new ChaseState(zombie));
                return;
            }
        }

        if(zombie.wayPoints.Length == 0)
        {
            return;
        }

        if (zombie.agent.pathPending == false && zombie.agent.remainingDistance < 0.5f)
        {
            zombie.currentWaypointIndex = (zombie.currentWaypointIndex +1) % zombie.wayPoints.Length;
            zombie.ChangeState(new IdleState(zombie));
        }
    }
}
