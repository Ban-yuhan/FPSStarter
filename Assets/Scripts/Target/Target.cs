using UnityEngine;

//데미지 적용 & 체력 0 이하로 떨어질 시 파괴처리
public class Target : MonoBehaviour
{
    [SerializeField]
    private float health = 50.0f;

    bool isAlive;

    private void Awake()
    {
        isAlive = true;
    }

    public void TakeDamage(float amount)
    {
        if (isAlive == true && health > 0)
        {
            health -= amount;
            Debug.Log(transform.name + " 남은 체력 : " + health);

            if (health <= 0)
            {
                Die();
            }
        }
        
    }

    void Die()
    {
        Destroy(gameObject);
        isAlive = false;
    }
}
