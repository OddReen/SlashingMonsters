using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    Animator animator;

    [Header("Health Stats")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    public bool isDead = false;

    [Header("Health Bar")]
    [SerializeField] GameObject healthbar;
    [SerializeField] float healthBarSpeed = 1f;
    float healthBarAmount = 1;

    float CurrentHealth
    {
        get { return currentHealth; }
        set { Mathf.Clamp(value, 0, maxHealth); }
    }
    private void Start()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Ragdoll"))
            {
                colliders[i].enabled = false;
                colliders[i].GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        currentHealth = maxHealth;
        if (healthbar != null)
        {
            healthbar.GetComponent<Image>().fillAmount = maxHealth;
        }
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
        if (isDead)
            return;
        animator.SetBool("isDamaged", true);
        StartCoroutine(EndBool("isDamaged"));
        StartCoroutine(HealthBarUpdate());
    }
    public void Heal(float amount)
    {
        if (isDead)
            return;
        currentHealth += amount;
        StartCoroutine(HealthBarUpdate());
    }
    public virtual void Die()
    {
        isDead = true;
        //animator.SetBool("isDead", true);
        //StartCoroutine(EndBool("isDead"));
        StartCoroutine(HealthBarUpdate());
        StartCoroutine(Ragdoll(true, 0));
    }
    public void Revive()
    {
        isDead = false;
        currentHealth = maxHealth;
        StartCoroutine(HealthBarUpdate());
        StartCoroutine(Ragdoll(false, 0));
    }
    IEnumerator HealthBarUpdate()
    {
        if (healthbar != null)
        {
            float targetHealth = currentHealth / maxHealth;

            while (healthBarAmount != targetHealth)
            {
                healthBarAmount = Mathf.MoveTowards(healthBarAmount, targetHealth, Time.deltaTime * healthBarSpeed);
                healthbar.GetComponent<Image>().fillAmount = healthBarAmount;
                yield return null;
            }

        }
    }
    public IEnumerator Ragdoll(bool isRagdoll, float startDelayTime)
    {
        yield return new WaitForSeconds(startDelayTime);
        animator.enabled = !isRagdoll;
        GetComponent<Rigidbody>().isKinematic = isRagdoll;
        GetComponent<Collider>().enabled = !isRagdoll;
        Collider[] colliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Ragdoll"))
            {
                colliders[i].enabled = isRagdoll;
                colliders[i].GetComponent<Rigidbody>().isKinematic = !isRagdoll;
            }
        }
    }
    IEnumerator EndBool(string boolName)
    {
        yield return new WaitForEndOfFrame();
        animator.SetBool(boolName, false);
    }
}
