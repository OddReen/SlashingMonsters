using System.Collections;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    Animator animator;
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    bool isDead = false;
    float CurrentHealth
    {
        get { return currentHealth; }
        set { Mathf.Clamp(value, 0, maxHealth); }
    }
    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    public void Damage(float amount)
    {
        if (isDead)
            return;
        currentHealth -= amount;
        Debug.Log(CurrentHealth);
        if (currentHealth <= 0)
            Die();
        animator.SetBool("isDamaged", true);
        StartCoroutine(EndBool("isDamaged"));
    }
    public void Health(float amount)
    {
        if (isDead)
            return;
        currentHealth += amount;
    }
    public void Die()
    {
        isDead = true;
        animator.SetBool("isDead", true);
        StartCoroutine(EndBool("isDead"));
    }
    IEnumerator EndBool(string boolName)
    {
        yield return new WaitForEndOfFrame();
        animator.SetBool(boolName, false);
    }
}
