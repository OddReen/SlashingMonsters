using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    [Header("Invincibility")]
    [SerializeField] bool isInvincible = false;

    [Header("References")]
    public CharacterBehaviour characterBehaviour;
    public Sounds sounds;

    [Header("Health Stats")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;

    [Header("Health Bar")]
    [SerializeField] protected GameObject healthBarBackground;
    [SerializeField] protected GameObject healthBar;
    [SerializeField] public GameObject healthBarLoss;
    [SerializeField] public float healthBarLossSpeed = 1;
    [SerializeField] public float healthBarLossAmount = 1;
    [SerializeField] public float healthBarLossTime = 1;

    [Header("Stun Stats")]
    [SerializeField] float maxStun = 100;
    [SerializeField] float currentStun;
    [SerializeField] float stunTime = 3;

    [Header("Stun Bar")]
    [SerializeField] protected GameObject stunBarBackground;
    [SerializeField] protected GameObject stunBar;
    Coroutine c_stunTime;
    Coroutine c_stunBarUpdate;
    Coroutine c_healthBarUpdate;

    float CurrentHealth
    {
        get { return currentHealth; }
        set { Mathf.Clamp(value, 0, maxHealth); }
    }
    private void Start()
    {
        sounds = GetComponent<Sounds>();

        characterBehaviour = GetComponent<CharacterBehaviour>();

        currentHealth = maxHealth;
        healthBar.GetComponent<Image>().fillAmount = maxHealth;

        currentStun = 0;
        stunBar.GetComponent<Image>().fillAmount = 0;
    }

    public void TakeDamage(float damageAmount)
    {
        if (!characterBehaviour.isDead)
        {
            TriggerAnimation("isDamaged");
            sounds.Hurt();
            if (!isInvincible)
            {
                currentHealth -= damageAmount;
                if (currentHealth <= 0)
                {
                    Die();
                    return;
                }
                HealthBarUpdate();
            }
        }
    }
    public void Heal(float healAmount)
    {
        if (characterBehaviour.isDead)
            return;
        currentHealth += healAmount;
        HealthBarUpdate();
    }

    public virtual void StackStun(float stunAmount)
    {
        if (!isInvincible && !characterBehaviour.isDead)
        {
            currentStun += stunAmount;
            if (currentStun >= maxStun)
                Stun();
            else
            {
                TriggerAnimation("isDamaged");
            }
            StunBarUpdate();
        }
    }
    public virtual void Stun()
    {
        characterBehaviour.isStunned = true;
        TriggerAnimation("isStunned");
        if (c_stunTime != null) StopCoroutine(c_stunTime);
        c_stunTime = StartCoroutine(C_Stun());
    }
    public virtual void StunEnd()
    {
        characterBehaviour.isStunned = false;
        currentStun = 0;
        StunBarUpdate();
    }
    IEnumerator C_Stun()
    {
        yield return new WaitForSeconds(stunTime);
        StunEnd();
    }

    public virtual void Die()
    {
        currentHealth = 0;
        characterBehaviour.isDead = true;
        StartCoroutine(C_HealthBarUpdate());
        TriggerAnimation("isDead");
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        for (int i = 0; i < characterBehaviour.actions.Count; i++)
        {
            characterBehaviour.actions[i].enabled = false;
        }
    }

    #region Health Bar
    public virtual void HealthBarUpdate()
    {
        if (c_healthBarUpdate != null) StopCoroutine(c_healthBarUpdate);
        c_healthBarUpdate = StartCoroutine(C_HealthBarUpdate());
    }
    IEnumerator C_HealthBarUpdate()
    {
        float health = (float)currentHealth / (float)maxHealth;
        healthBar.GetComponent<Image>().fillAmount = health;

        yield return new WaitForSeconds(healthBarLossTime);

        while (healthBarLossAmount != health)
        {
            healthBarLossAmount = Mathf.MoveTowards(healthBarLossAmount, health, Time.deltaTime * healthBarLossSpeed);
            healthBarLoss.GetComponent<Image>().fillAmount = healthBarLossAmount;
            yield return null;
        }
    }
    #endregion
    #region Stun Bar
    public virtual void StunBarUpdate()
    {
        if (c_stunBarUpdate != null) StopCoroutine(c_stunBarUpdate);
        c_stunBarUpdate = StartCoroutine(C_StunBarUpdate());
    }
    IEnumerator C_StunBarUpdate()
    {
        float stun = (float)currentStun / (float)maxStun;
        stunBar.GetComponent<Image>().fillAmount = stun;
        yield return null;
    }
    #endregion
    #region Trigger Animation
    public void TriggerAnimation(string boolName)
    {
        StartCoroutine(C_TriggerAnimation(boolName));
    }
    IEnumerator C_TriggerAnimation(string boolName)
    {
        characterBehaviour.animator.SetBool(boolName, true);
        yield return new WaitForSeconds(.1f);
        characterBehaviour.animator.SetBool(boolName, false);
    }
#endregion
}
