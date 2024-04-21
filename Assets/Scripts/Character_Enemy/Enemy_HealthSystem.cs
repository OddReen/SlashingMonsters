using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : HealthSystem
{
    public override void Die()
    {
        base.Die();
        characterBehaviour.StopAllCoroutines();
        healthBarBackground.SetActive(false);
        stunBarBackground.SetActive(false);
    }
}
