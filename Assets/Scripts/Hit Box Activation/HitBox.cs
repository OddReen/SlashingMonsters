using Cinemachine;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitBox : MonoBehaviour
{
    public EventReference slash;
    public EventReference execution;
    public EventReference swing;

    [SerializeField] protected string targetTypeTag;

    [SerializeField] protected CinemachineImpulseSource cinemachineImpulseSource;

    protected CharacterBehaviour characterBehaviour;
    protected List<Collider> enemiesHit = new List<Collider>();
    protected Collider hitBox;

    [SerializeField] protected GameObject bloodPref;

    public virtual void Start()
    {
        characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
        hitBox = GetComponent<Collider>();
        hitBox.enabled = false;
    }
    public virtual void EnableHitBox()
    {
        FMODUnity.RuntimeManager.PlayOneShot(swing, transform.position);
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
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag(targetTypeTag))
    //    {
    //        for (int i = 0; i < enemiesHit.Count; i++)
    //        {
    //            if (other == enemiesHit[i])
    //            {
    //                return;
    //            }
    //        }

    //        Player_CameraController.Instance.CameraShake(cinemachineImpulseSource);
    //        enemiesHit.Add(other);

    //        Instantiate(bloodPref, other.ClosestPointOnBounds(hitBox.transform.position), transform.rotation);

    //        other.GetComponent<HealthSystem>().TakeDamage(characterBehaviour.damageAmount);

    //        if (other.GetComponent<HealthSystem>().isDead)
    //        {
    //            FMODUnity.RuntimeManager.PlayOneShot(execution, transform.position);
    //        }
    //        else
    //        {
    //            FMODUnity.RuntimeManager.PlayOneShot(slash, transform.position);
    //        }
    //    }
    //}
}
