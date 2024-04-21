using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("References")]
    public CharacterBehaviour characterBehaviour;

    [Header("Health Stats")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;

    [Header("Health Bar")]
    [SerializeField] protected GameObject healthBarBackground;
    [SerializeField] protected GameObject healthBar;
    [SerializeField] float healthBarSpeed = 1;
    [SerializeField] float healthBarAmount = 1;

    [Header("Stun Stats")]
    [SerializeField] float maxStun = 100;
    [SerializeField] float currentStun;
    [SerializeField] float stunTime = 3;

    [Header("Stun Bar")]
    [SerializeField] protected GameObject stunBarBackground;
    [SerializeField] protected GameObject stunBar;
    [SerializeField] float stunBarSpeed = 1;
    [SerializeField] float stunBarAmount = 0;
    Coroutine c_stunTime;
    Coroutine c_stunBarUpdate;

    float CurrentHealth
    {
        get { return currentHealth; }
        set { Mathf.Clamp(value, 0, maxHealth); }
    }
    private void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();

        currentHealth = maxHealth;
        healthBar.GetComponent<Image>().fillAmount = maxHealth;

        currentStun = 0;
        stunBar.GetComponent<Image>().fillAmount = 0;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0)
            Die();
        if (characterBehaviour.isDead)
            return;
        StartCoroutine(TriggerAnimation("isDamaged"));
        StartCoroutine(C_HealthBarUpdate());
    } // Take Health
    public void Heal(float healAmount)
    {
        if (characterBehaviour.isDead)
            return;
        currentHealth += healAmount;
        StartCoroutine(C_HealthBarUpdate());
    } // Heal Health
    IEnumerator C_HealthBarUpdate()
    {
        if (healthBar != null)
        {
            float targetHealth = currentHealth / maxHealth;

            while (healthBarAmount != targetHealth)
            {
                healthBarAmount = Mathf.MoveTowards(healthBarAmount, targetHealth, Time.deltaTime * healthBarSpeed);
                healthBar.GetComponent<Image>().fillAmount = healthBarAmount;
                yield return null;
            }

        }
    } // Health Bar

    public void Stun(float stunAmount)
    {
        if (characterBehaviour.isDead)
            return;
        currentStun += stunAmount;

        if (currentStun >= maxStun)
        {
            if (c_stunTime != null) StopCoroutine(c_stunTime);
            c_stunTime = StartCoroutine(C_Stun());
        }

        if (c_stunBarUpdate != null) StopCoroutine(c_stunBarUpdate);
        c_stunBarUpdate = StartCoroutine(C_StunBarUpdate());
    } // Stun
    IEnumerator C_Stun()
    {
        characterBehaviour.isStunned = true;
        characterBehaviour.animator.SetBool("isStunned", true);
        yield return new WaitForSeconds(stunTime);
        characterBehaviour.isStunned = false;
        characterBehaviour.animator.SetBool("isStunned", false);

        currentStun = 0;
        if (c_stunBarUpdate != null) StopCoroutine(c_stunBarUpdate);
        c_stunBarUpdate = StartCoroutine(C_StunBarUpdate());
    } // Time in stun
    IEnumerator C_StunBarUpdate()
    {
        if (stunBar != null)
        {
            float targetStun = currentStun / maxStun;

            while (stunBarAmount != targetStun)
            {
                stunBarAmount = Mathf.MoveTowards(stunBarAmount, targetStun, Time.deltaTime * stunBarSpeed);
                stunBar.GetComponent<Image>().fillAmount = stunBarAmount;
                yield return null;
            }

        }
    } // Stun Bar

    public virtual void Die()
    {
        characterBehaviour.isDead = true;
        StartCoroutine(C_HealthBarUpdate());
        StartCoroutine(TriggerAnimation("isDead"));
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        for (int i = 0; i < characterBehaviour.actions.Count; i++)
        {
            characterBehaviour.actions[i].enabled = false;
        }
    }
    IEnumerator TriggerAnimation(string boolName)
    {
        characterBehaviour.animator.SetBool(boolName, true);
        yield return new WaitForEndOfFrame();
        characterBehaviour.animator.SetBool(boolName, false);
    }
}
