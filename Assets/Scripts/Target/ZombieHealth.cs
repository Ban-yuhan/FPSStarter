using UnityEngine;

public class ZombieHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth = 100f;

    [SerializeField]
    private float currentHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0f)
        {
            Die();
        }
    }


    void Die()
    {
        Destroy(gameObject);
    }
}
