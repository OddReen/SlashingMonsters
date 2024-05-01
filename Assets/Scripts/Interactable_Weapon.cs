using Cinemachine;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Weapon : Interactable_Equipables
{
    [Header("Sounds")]
    public EventReference slash;
    public EventReference execution;
    public EventReference swing;

    [Header("References")]
    [SerializeField] GameObject bloodPref;
    CharacterBehaviour characterBehaviour;
    [SerializeField] Collider hitBox;
    CinemachineImpulseSource cinemachineImpulseSource;
    Interactable_Equipables interactable;

    [Header("Target Type")]
    [SerializeField] string targetTypeTag;

    List<Collider> enemiesHit = new List<Collider>();

    public override void Action(CharacterBehaviour_Player characterBehaviour_Player)
    {
        if (!characterBehaviour_Player.hasWeapon)
        {
            characterBehaviour_Player.GetComponent<Equipment>().EquipLeft(transform);
        }
    }
    public void Start()
    {
        interactable = GetComponentInParent<Interactable_Equipables>();
        characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        hitBox.enabled = false;
    }
    public void EnableHitBox()
    {
        RuntimeManager.PlayOneShot(swing, transform.position);
        hitBox.enabled = true;
    }
    public void DisableHitBox()
    {
        hitBox.enabled = false;
        if (enemiesHit.Count != 0) enemiesHit.Clear();
    }
    private void OnTriggerEnter(Collider target)
    {
        if (target.CompareTag(targetTypeTag))
        {
            for (int i = 0; i < enemiesHit.Count; i++)
            {
                if (target == enemiesHit[i])
                {
                    return;
                }
            }

            enemiesHit.Add(target);

            CharacterBehaviour targetBehaviour = target.GetComponent<CharacterBehaviour>();

            if (targetBehaviour.isParrying)
            {
                characterBehaviour.healthSystem.Stun(targetBehaviour.stunTimeAmount);
                return;
            }

            Instantiate(bloodPref, target.ClosestPointOnBounds(hitBox.transform.position), transform.rotation);
            targetBehaviour.healthSystem.TakeDamage(interactable.damageAmount);
            targetBehaviour.healthSystem.Stun(interactable.stunAmount);

            Player_CameraController.Instance.CameraShake(cinemachineImpulseSource);

            if (targetBehaviour.isDead)
                RuntimeManager.PlayOneShot(execution, transform.position);
            else
                RuntimeManager.PlayOneShot(slash, transform.position);
        }
    }
}
