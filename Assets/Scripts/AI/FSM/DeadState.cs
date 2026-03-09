using UnityEngine;

public class DeadState : ZombieState
{
    public DeadState(ZombieController zonbieController) : base(zonbieController)
    {

    }


    public override void Enter()
    {
        Debug.Log("죽었다");

        zombie.agent.isStopped = true;
        zombie.agent.enabled = false;
        zombie.GetComponent<Collider>().enabled = false;

        MissionEventBus.PublishEnemyKilled(); //DeadState상태에 진입 했을때 죽었다고 통보

        if (zombie.ragdoll != null)
        {
            zombie.ragdoll.EnableRagdoll();
        }

        Object.Destroy(zombie.gameObject, 5.0f);
    }
}
