using UnityEngine;

public class IdleState : ZombieState
{
    public IdleState(ZombieController zombieController) : base(zombieController) //부모클래스의 zombieController의 생성자를 호출
    {

    }


    public override void Enter() //부모클래스의 함수를 사용하지 않고 함수를 재정의하여 사용
    {
        zombie.idleTimer = 0.0f;
        zombie.agent.ResetPath();
        zombie.animator.SetBool("Move", false);
    }


    public override void Update()
    {
        //감지 체크
        if(zombie.targetPlayer != null)
        {
            float dist = Vector3.Distance(zombie.transform.position, zombie.targetPlayer.transform.position);

            if(zombie.DetectPlayer(dist) == true)
            {
                zombie.ChangeState(new ChaseState(zombie));
                return;
            }
        }

        zombie.idleTimer += Time.deltaTime;
        if(zombie.idleTimer >= zombie.idleDuration)
        {
            zombie.ChangeState(new PatrolState  (zombie));
        }
    }
}
