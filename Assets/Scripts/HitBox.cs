using Cinemachine;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    enum AttackType
    {
        Attack,
        Kick
    }
    [SerializeField] AttackType attackType;

    [Header("Sounds")]
    public EventReference slash;
    public EventReference execution;
    public EventReference swing;

    [Header("References")]
    [SerializeField] GameObject bloodPref;
    CharacterBehaviour characterBehaviour;
    Collider hitBox;
    CinemachineImpulseSource cinemachineImpulseSource;
    Interactable_Weapon interactable_Weapon;

    [Header("Target Type")]
    [SerializeField] string targetTypeTag;

    List<Collider> enemiesHit = new List<Collider>();

    public virtual void Start()
    {
        interactable_Weapon = GetComponentInParent<Interactable_Weapon>();
        characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        hitBox = GetComponent<Collider>();
        hitBox.enabled = false;
    }
    public virtual void EnableHitBox()
    {
        RuntimeManager.PlayOneShot(swing, transform.position);
        hitBox.enabled = true;
    }
    public virtual void DisableHitBox()
    {
        hitBox.enabled = false;
        if (enemiesHit.Count != 0)
        {
            enemiesHit.Clear();
        }
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
            switch (attackType)
            {
                case AttackType.Attack:
                    targetBehaviour.healthSystem.TakeDamage(interactable_Weapon.damageAmount);
                    Instantiate(bloodPref, target.ClosestPointOnBounds(hitBox.transform.position), transform.rotation);
                    break;
                case AttackType.Kick:
                    targetBehaviour.healthSystem.TakeDamage(characterBehaviour.kickDamageAmount);
                    targetBehaviour.healthSystem.Stun(characterBehaviour.stunTimeAmount);
                    break;
                default:
                    break;
            }

            Player_CameraController.Instance.CameraShake(cinemachineImpulseSource);

            if (targetBehaviour.isDead)
                RuntimeManager.PlayOneShot(execution, transform.position);
            else
                RuntimeManager.PlayOneShot(slash, transform.position);
        }
    }
}
