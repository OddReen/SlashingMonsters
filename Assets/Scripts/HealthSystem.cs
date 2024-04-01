using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    CharacterBehaviour characterBehaviour;

    [Header("Health Stats")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;


    [Header("Health Bar")]
    [SerializeField] protected GameObject healthBarBackground;
    [SerializeField] protected GameObject healthbar;
    [SerializeField] float healthBarSpeed = 1f;
    [SerializeField] float healthBarAmount = 1;

    Coroutine c_stunTime;

    float CurrentHealth
    {
        get { return currentHealth; }
        set { Mathf.Clamp(value, 0, maxHealth); }
    }
    private void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
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
    }
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
            Die();
        if (characterBehaviour.isDead)
            return;
        StartCoroutine(TriggerAnimation("isDamaged"));
        StartCoroutine(HealthBarUpdate());
    }
    public void Heal(float healAmount)
    {
        if (characterBehaviour.isDead)
            return;
        currentHealth += healAmount;
        StartCoroutine(HealthBarUpdate());
    }
    public void Stun(float timeAmount)
    {
        if (characterBehaviour.isDead)
            return;
        if (c_stunTime != null)
            StopCoroutine(c_stunTime);
        c_stunTime = StartCoroutine(StunTime(timeAmount));
    }
    IEnumerator StunTime(float timeAmount)
    {
        characterBehaviour.animator.SetBool("isStunned", true);
        characterBehaviour.isStunned = true;
        yield return new WaitForSeconds(timeAmount);
        characterBehaviour.animator.SetBool("isStunned", false);
        characterBehaviour.isStunned = false;
    }
    public virtual void Die()
    {
        characterBehaviour.isDead = true;
        StartCoroutine(HealthBarUpdate());
        Ragdoll(true);
        for (int i = 0; i < characterBehaviour.actions.Count; i++)
        {
            characterBehaviour.actions[i].enabled = false;
        }
    }
    public void Revive()
    {
        characterBehaviour.isDead = false;
        currentHealth = maxHealth;
        StartCoroutine(HealthBarUpdate());
        Ragdoll(false);
        for (int i = 0; i < characterBehaviour.actions.Count; i++)
        {
            characterBehaviour.actions[i].enabled = true;
        }
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
    public void Ragdoll(bool isRagdoll)
    {
        characterBehaviour.animator.enabled = !isRagdoll;
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
    IEnumerator TriggerAnimation(string boolName)
    {
        characterBehaviour.animator.SetBool(boolName, true);
        yield return new WaitForEndOfFrame();
        characterBehaviour.animator.SetBool(boolName, false);
    }
}
