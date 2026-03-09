using UnityEngine;

//다른 상태클래스의 부모역할을 할 클래스
public class ZombieState
{
    protected ZombieController zombie;

    public ZombieState(ZombieController zombieController)
    {
        zombie = zombieController;
    }


    //virtual : 가상 함수
    // 해당 클래스를 상속하는 자식 클래스에서 이 함수를 재정의 할 수 있게 만들어 줌
    public virtual void Enter()
    {

    }


    public virtual void Update()
    {

    }


    public virtual void Exit()
    {

    }
}
