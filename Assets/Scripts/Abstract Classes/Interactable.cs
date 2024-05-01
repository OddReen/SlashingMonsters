using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float distanceToInteract;
    public virtual void Action(CharacterBehaviour_Player characterBehaviour_Player)
    {

    }
}
