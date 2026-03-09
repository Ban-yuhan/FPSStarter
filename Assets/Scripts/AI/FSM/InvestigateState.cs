using UnityEngine;

public class InvestigateState : ZombieState
{
    private Vector3 targetPos;
    private float stayTimer = 0.0f;
    private float stayDuration = 3.0f;

    public InvestigateState(ZombieController zombieController, Vector3 noisePos) : base(zombieController)
    {
        targetPos = noisePos;
    }


    public override void Enter()
    {
        zombie.agent.isStopped = false;
        zombie.agent.SetDestination(targetPos);
    }
}
