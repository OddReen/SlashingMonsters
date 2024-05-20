using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_HealthSystem : HealthSystem
{
    [Header("Interactable Weapon")]
    [SerializeField] Weapon interactable_Weapon;

    public override void Stun()
    {
        base.Stun();
        interactable_Weapon.canInteract = true;
    }
    public override void StunEnd()
    {
        base.StunEnd();
        interactable_Weapon.canInteract = false;
    }
    public override void Die()
    {
        base.Die();
        characterBehaviour.StopAllCoroutines();
        healthBarBackground.SetActive(false);
        stunBarBackground.SetActive(false);
        Destroy(GetComponent<CharacterBehaviour_Enemy>());

    }
}
