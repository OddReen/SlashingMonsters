using Cinemachine;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Interactable_Equipables
{
    public int weaponHash;

    [Header("Boolean")]
    public bool isHitBoxActive;

    [Header("Durability")]
    public int maxDurability = 5;
    public int currentDurability;
    public int CurrentDurability
    {
        get { return currentDurability; }
        set { Mathf.Clamp(value, 0, maxDurability); }
    }
    Coroutine c_DurabilityBarUpdate;

    [Header("Sounds")]
    public EventReference slash;
    public EventReference execution;
    public EventReference swing;

    [Header("References")]
    public Equipment equipment;
    [SerializeField] GameObject bloodPref;
    public CharacterBehaviour characterBehaviour;
    [SerializeField] Collider hitBox;
    CinemachineImpulseSource cinemachineImpulseSource;
    Interactable_Equipables interactable;

    [Header("Target Type")]
    [SerializeField] public string targetTypeTag;

    List<GameObject> enemiesHit = new List<GameObject>();

    public override void Action(CharacterBehaviour_Player characterBehaviour_Player)
    {
        if (!characterBehaviour_Player.hasWeapon && characterBehaviour.isStunned)
        {
            characterBehaviour_Player.GetComponent<Equipment>().EquipWeapon(gameObject);
        }
    }
    public void Start()
    {
        currentDurability = maxDurability;
        canInteract = false;
        interactable = GetComponentInParent<Interactable_Equipables>();
        characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        hitBox.enabled = false;
    }
    public void EnableHitBox()
    {
        isHitBoxActive = true;
        RuntimeManager.PlayOneShot(swing, transform.position);
        hitBox.enabled = true;
    }
    public void DisableHitBox()
    {
        isHitBoxActive = false;
        hitBox.enabled = false;
        if (enemiesHit.Count != 0) enemiesHit.Clear();
    }
    public void DurabilityLoss()
    {
        currentDurability--;

        DurabilityBarUpdate();

        if (currentDurability <= 0)
        {
            GameManager.Instance.player.GetComponent<CharacterBehaviour_Player>().hasWeapon = false;
            GameManager.Instance.player.GetComponent<Equipment>().durabilityBarBackground.SetActive(false);
            Destroy(gameObject);
        }
    }
    public void DurabilityBarUpdate()
    {
        if (c_DurabilityBarUpdate != null) StopCoroutine(c_DurabilityBarUpdate);
        c_DurabilityBarUpdate = StartCoroutine(C_DurabilityBarUpdate());
    }
    IEnumerator C_DurabilityBarUpdate()
    {
        float targetDurabilty = (float)currentDurability / (float)maxDurability;
        equipment.durabilityBar.GetComponent<Image>().fillAmount = targetDurabilty;

        yield return new WaitForSeconds(equipment.durabilityBarLossTime);

        while (equipment.durabilityBarLossAmount != targetDurabilty)
        {
            equipment.durabilityBarLossAmount = Mathf.MoveTowards(equipment.durabilityBarLossAmount, targetDurabilty, Time.deltaTime * equipment.durabilityBarLossSpeed);
            equipment.durabilityBarLoss.GetComponent<Image>().fillAmount = equipment.durabilityBarLossAmount;
            yield return null;
        }
    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag(targetTypeTag))
        {
            GameObject targetGameObject = target.gameObject;
            for (int i = 0; i < enemiesHit.Count; i++)
            {
                if (targetGameObject == enemiesHit[i])
                {
                    return;
                }
            }

            if (equipment != null)
            {
                DurabilityLoss();
            }

            enemiesHit.Add(targetGameObject);

            CharacterBehaviour targetBehaviour = targetGameObject.GetComponent<CharacterBehaviour>();

            if (targetBehaviour.isParrying)
            {
                characterBehaviour.healthSystem.StackStun(targetBehaviour.parryStunAmount);
                return;
            }

            Instantiate(bloodPref, targetGameObject.GetComponent<Collider>().ClosestPointOnBounds(hitBox.transform.position), transform.rotation);
            targetBehaviour.healthSystem.TakeDamage(interactable.damageAmount);
            targetBehaviour.healthSystem.StackStun(interactable.stunAmount);

            Player_CameraController.Instance.CameraShake(cinemachineImpulseSource);

            if (targetBehaviour.isDead)
                RuntimeManager.PlayOneShot(execution, transform.position);
            else
                RuntimeManager.PlayOneShot(slash, transform.position);
        }
    }
}
