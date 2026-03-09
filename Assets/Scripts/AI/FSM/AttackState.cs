using UnityEngine;

public class AttackState : ZombieState
{
    public AttackState(ZombieController zombieController) : base(zombieController)
    {

    }


    public override void Enter()
    {
        zombie.agent.isStopped = true;
    }

    public override void Update()
    {
        if(zombie.targetPlayer == null)
        {
            return;
        }

        float distanceToPlayer = Vector3.Distance(zombie.transform.position, zombie.targetPlayer.position);
        
        if(distanceToPlayer > zombie.attackRange)
        {
            zombie.ChangeState(new ChaseState(zombie));
            return;
        }

        Vector3 targetPos = new Vector3(zombie.targetPlayer.position.x, zombie.transform.position.y, zombie.targetPlayer.position.z);
        zombie.transform.LookAt(targetPos);

        if(Time.time >= zombie.LastAttackTime + zombie.attackRate)
        {
            zombie.LastAttackTime = Time.time;
            zombie.animator.SetTrigger("Attack");
        }
    }
}
