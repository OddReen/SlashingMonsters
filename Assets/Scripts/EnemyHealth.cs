using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    [SerializeField] float bodyLifeSpan = 10.0f;
    public override void Die()
    {
        base.Die();
        Invoke("Revive", 5);
    }
    public IEnumerator BodyLifeSpan()
    {
        yield return new WaitForSeconds(bodyLifeSpan);
        Destroy(gameObject);
    }
}
