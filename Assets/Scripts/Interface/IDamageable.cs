using UnityEngine;

public interface IDamageable
{
    //함수 선언만 하고 구현은 하지 않음.
    void TakeDamage(float damageAmount);
    //해당 Interface를 상속받는 클래스는 TakeDamage라는 이름의 함수를 가지고 있어야 한다 라고 강제하는 규칙을 정의
}
