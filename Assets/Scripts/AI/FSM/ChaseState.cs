using UnityEngine;

public class ChaseState : ZombieState
{
  public ChaseState(ZombieController zombieController) : base(zombieController)
  {

  }

    public override void Enter()
    {
        zombie.animator.SetBool("Move", true);
        
        if(zombie.agent.enabled == true)
        {
            zombie.agent.isStopped = false;
        }
    }


    public override void Update()
    {
        if (zombie.targetPlayer == null)
        {
            return;
        }

        if(zombie.agent.enabled == true)
        {
            zombie.agent.SetDestination(zombie.targetPlayer.position);
        }

        float distanceToPlayer = Vector3.Distance(zombie.transform.position, zombie.targetPlayer.position);

        if(distanceToPlayer <= zombie.attackRange)
        {
            zombie.ChangeState(new AttackState(zombie));
        }
        else if(distanceToPlayer > zombie.viewDistance)
        {
            zombie.ChangeState(new PatrolState(zombie));
        }
    }
}
