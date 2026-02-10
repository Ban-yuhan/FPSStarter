using UnityEngine;
using System; //옵저버 패턴을 위해 System 네임스페이스 추가

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth = 100f;

    private float currentHealth;

    //플레이어의 체력이 변경될 때마다 현재 체력 비율(0.0f ~ 1.0f)을 전달
    public event Action<float> OnHealthChanged; //float을 파라미터로 받은 이벤트. 이 이벤트에 등록할 수 있는 함수는 float 타입을 파라미터로 받아야 함
    public event Action OnDeath; //플레이어가 사망했을 때 호출되는 이벤트. 어떠한 파라미터도 받지 않음


    private void Start()
    {
        currentHealth = maxHealth;

        if(OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(1.0f); //초기 체력 비율을 알리기 위해 1.0f 전달. 1.0f 대신 currentHealth / maxHealth 사용 가능
        }
    }


    public void TakeDamage(float damage)
    {
        if(currentHealth <=0)
        {
            return; //이미 사망한 경우 리턴
        }

        currentHealth -= damage;

        float healthPercent = currentHealth / maxHealth;
        
        if(currentHealth <= 0)
        {
            healthPercent = 0f; //체력 비율이 음수가 되는 것을 방지
        }

        if(OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(healthPercent); //체력 변화 알림
        }

        if(currentHealth <=0)
        {
            Die();
        }
    }


    void Die()
    {
        currentHealth = 0f;

        if(OnDeath != null)
        {
            OnDeath.Invoke(); //사망 알림
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth; //최대 체력 초과 방지
        }

        float healthPercent = currentHealth / maxHealth;

        if(OnHealthChanged != null)
        {
            OnHealthChanged.Invoke(healthPercent); //체력 변화 알림
        }
    }
}
