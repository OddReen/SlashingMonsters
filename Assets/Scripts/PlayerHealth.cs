using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : HealthSystem
{
    public override void Die()
    {
        base.Die();
        Invoke("Revive", 5);
    }
}
