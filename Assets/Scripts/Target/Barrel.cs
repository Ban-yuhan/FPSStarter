using Unity.VisualScripting;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float BarrelMaxHP = 10f;

    [SerializeField]
    private float BarrelCurHP;

    private void Start()
    {
        BarrelCurHP = BarrelMaxHP;
    }

    public void TakeDamage(float DamageAmount)
    {
        BarrelCurHP -= DamageAmount;
        if (BarrelCurHP <= 0f)
        {
            Break();
        }
    }

    void Break()
    {
        //주변에 데미지를 주는 기능 구현
        Destroy(gameObject);
    }
}
