using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActions : MonoBehaviour
{
    [Header("Action Stats")]
    [SerializeField] protected float elapsedTime = 0f;

    [Header("Action Properties")]
    [SerializeField] protected string actionTag = "";

    protected CharacterBehaviour_Player characterBehaviour_Player;

    protected virtual void Awake()
    {
        characterBehaviour_Player = GetComponent<CharacterBehaviour_Player>();
    }
    public virtual void Action()
    {

    }
}
