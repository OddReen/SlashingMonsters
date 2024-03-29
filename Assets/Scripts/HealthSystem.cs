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
    public bool isDead = false;

    [Header("Health Bar")]
    [SerializeField] protected GameObject healthBarBackground;
    [SerializeField] protected GameObject healthbar;
    [SerializeField] float healthBarSpeed = 1f;
    [SerializeField] float healthBarAmount = 1;

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
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Die();
        if (isDead)
            return;
        characterBehaviour.animator.SetBool("isDamaged", true);
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
        Ragdoll(true);
        for (int i = 0; i < characterBehaviour.actions.Count; i++)
        {
            characterBehaviour.actions[i].enabled = false;
        }
    }
    public void Revive()
    {
        isDead = false;
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
    IEnumerator EndBool(string boolName)
    {
        yield return new WaitForEndOfFrame();
        characterBehaviour.animator.SetBool(boolName, false);
    }
}
