using Cinemachine;
using FMODUnity;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBox_Weapon : HitBox
{
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

            if (target.GetComponent<CharacterBehaviour>().isParrying)
            {
                characterBehaviour.healthSystem.Stun(targetBehaviour.stunTimeAmount);
                return;
            }

            Player_CameraController.Instance.CameraShake(cinemachineImpulseSource);
            Instantiate(bloodPref, target.ClosestPointOnBounds(hitBox.transform.position), transform.rotation);
            targetBehaviour.healthSystem.TakeDamage(characterBehaviour.weaponDamageAmount);

            if (targetBehaviour.isDead)
            {
                FMODUnity.RuntimeManager.PlayOneShot(execution, transform.position);
            }
            else
            {
                FMODUnity.RuntimeManager.PlayOneShot(slash, transform.position);
            }
        }
    }
}
