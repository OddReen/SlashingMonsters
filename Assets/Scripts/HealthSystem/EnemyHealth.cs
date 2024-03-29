using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : HealthSystem
{
    [SerializeField] float bodyLifeSpan = 10.0f;
    public override void Die()
    {
        base.Die();
        healthBarBackground.SetActive(false);
    }
    public IEnumerator BodyLifeSpan()
    {
        yield return new WaitForSeconds(bodyLifeSpan);
        Destroy(gameObject);
    }
}
