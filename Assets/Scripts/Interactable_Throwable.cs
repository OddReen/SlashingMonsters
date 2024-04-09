using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Throwable : Interactable
{
    [SerializeField] float damageAmount;
    public override void Action(CharacterBehaviour_Player characterBehaviour_Player)
    {
        if (!characterBehaviour_Player.hasThrowable)
        {
            characterBehaviour_Player.GetComponent<Equipment>().EquipRight(transform);
        }
    }
}