using UnityEngine;

public class DeadState : ZombieState
{
    public DeadState(ZombieController zonbieController) : base(zonbieController)
    {

    }


    public override void Enter()
    {
        zombie.agent.isStopped = true;
        zombie.agent.enabled = false;
        zombie.GetComponent<Collider>().enabled = false;

        if (zombie.ragdoll != null)
        {
            zombie.ragdoll.EnableRagdoll();
        }

        Object.Destroy(zombie.gameObject, 5.0f);
    }
}
