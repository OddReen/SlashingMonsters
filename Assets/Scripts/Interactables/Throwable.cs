using Cinemachine;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Interactable_Equipables
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
        if (!characterBehaviour_Player.hasThrowable)
        {
            characterBehaviour_Player.GetComponent<Equipment>().EquipThrowable(transform);
        }
    }
    public void Start()
    {
        canInteract = true;
        interactable = GetComponentInParent<Interactable_Equipables>();
        characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        hitBox.enabled = false;
    }
    public void EnableHitBox()
    {
        StartCoroutine(C_HitBox());
    }
    public IEnumerator C_HitBox()
    {
        RuntimeManager.PlayOneShot(swing, transform.position);
        hitBox.enabled = true;
        yield return new WaitForSeconds(3);
        Rigidbody rb = GetComponentInParent<Rigidbody>();
        rb.isKinematic = true;
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
                characterBehaviour.healthSystem.StackStun(targetBehaviour.parryStunAmount);
                return;
            }

            Instantiate(bloodPref, target.ClosestPointOnBounds(hitBox.transform.position), transform.rotation);
            targetBehaviour.healthSystem.TakeDamage(interactable.damageAmount);
            targetBehaviour.healthSystem.StackStun(interactable.stunAmount);

            Player_CameraController.Instance.CameraShake(cinemachineImpulseSource);

            if (targetBehaviour.isDead)
                RuntimeManager.PlayOneShot(execution, transform.position);
            else
                RuntimeManager.PlayOneShot(slash, transform.position);
            Destroy(gameObject);
        }
    }
}