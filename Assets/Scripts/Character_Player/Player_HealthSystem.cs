using System.Collections;
using UnityEngine;

public class Player_HealthSystem : HealthSystem
{
    public override void Die()
    {
        base.Die();
        GameManager.Instance.Restart();
    }
}
