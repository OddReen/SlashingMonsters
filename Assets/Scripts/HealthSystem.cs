using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth = 0;
    float CurrentHealth
    {
        get { return currentHealth; }
        set { Mathf.Clamp(value, 0, maxHealth); }
    }
    private void Start()
    {
        currentHealth = maxHealth;
    }
    public void Damage(float amount)
    {
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
            Die();
    }
    public void Heal(float amount)
    {
        CurrentHealth += amount;
    }
    public void Die()
    {
        Debug.Log("Death");
    }
}
