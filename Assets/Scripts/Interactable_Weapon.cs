using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Weapon : Interactable
{
    [SerializeField] public float damageAmount;
    public override void Action(CharacterBehaviour_Player characterBehaviour_Player)
    {
        if (!characterBehaviour_Player.hasWeapon)
        {
            characterBehaviour_Player.GetComponent<Equipment>().EquipLeft(transform);
        }
    }
}
