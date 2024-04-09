using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Activatable : Interactable
{
    public override void Action(CharacterBehaviour_Player characterBehaviour_Player)
    {
        characterBehaviour_Player.healthSystem.Die();
    }
}
