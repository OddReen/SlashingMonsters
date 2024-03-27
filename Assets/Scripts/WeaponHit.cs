using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    public EventReference slash;
    public EventReference execution;
    public EventReference swing;

    [SerializeField] string targetTypeTag;

    CharacterBehaviour characterBehaviour;
    List<Collider> enemiesHit = new List<Collider>();
    Collider hitBox;
    Light hitBoxLight;

    [SerializeField] GameObject bloodPref;

    private void Start()
    {
        characterBehaviour = GetComponentInParent<CharacterBehaviour>();
        hitBox = GetComponent<Collider>();
        hitBoxLight = GetComponent<Light>();
        hitBox.enabled = false;
        hitBoxLight.enabled = false;
    }
    public void EnableHitBox()
    {
        FMODUnity.RuntimeManager.PlayOneShot(swing, transform.position);
        hitBox.enabled = true;
        hitBoxLight.enabled = true;
    }
    public void DisableHitBox()
    {
        hitBox.enabled = false;
        hitBoxLight.enabled = false;
        ClearEnemyHitList();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTypeTag))
        {
            for (int i = 0; i < enemiesHit.Count; i++)
            {
                if (other == enemiesHit[i])
                {
                    return;
                }
            }
            enemiesHit.Add(other);

            Instantiate(bloodPref, other.ClosestPointOnBounds(hitBox.transform.position), transform.rotation);

            other.GetComponent<HealthSystem>().TakeDamage(characterBehaviour.damageAmount);

            if (other.GetComponent<HealthSystem>().isDead)
            {
                FMODUnity.RuntimeManager.PlayOneShot(execution, transform.position);
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(slash, transform.position);
            }
        }
    }
    public void ClearEnemyHitList()
    {
        if (enemiesHit.Count != 0)
        {
            enemiesHit.Clear();
        }
    }
}
