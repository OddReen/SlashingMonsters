using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHit : MonoBehaviour
{
    CombatHandler combatHandler;
    List<Collider> enemiesHit = new List<Collider>();
    private void Start()
    {
        combatHandler = GetComponentInParent<CombatHandler>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            for (int i = 0; i < enemiesHit.Count; i++)
            {
                if (other == enemiesHit[i])
                {
                    return;
                }
            }
            enemiesHit.Add(other);
            other.GetComponent<HealthSystem>().Damage(combatHandler.damageAmount);
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
