using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox_Kick : HitBox
{
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

            Player_CameraController.Instance.CameraShake(cinemachineImpulseSource);
            enemiesHit.Add(other);

            Instantiate(bloodPref, other.ClosestPointOnBounds(hitBox.transform.position), transform.rotation);

            other.GetComponent<HealthSystem>().TakeDamage(characterBehaviour.kickDamageAmount);
            other.GetComponent<HealthSystem>().Stun(characterBehaviour.stunTimeAmount);

            if (other.GetComponent<CharacterBehaviour>().isDead)
                FMODUnity.RuntimeManager.PlayOneShot(execution, transform.position);
            else
                FMODUnity.RuntimeManager.PlayOneShot(slash, transform.position);
        }
    }
}
