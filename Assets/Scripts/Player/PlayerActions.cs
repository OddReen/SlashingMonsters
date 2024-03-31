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
    public IEnumerator TriggerAnimation()
    {
        characterBehaviour_Player.animator.SetBool(actionTag, true);
        yield return new WaitForEndOfFrame();
        characterBehaviour_Player.animator.SetBool(actionTag, false);
    }
    public void InitializeRootMotion()
    {
        characterBehaviour_Player.player_Movement.currentSpeed = 0;
        characterBehaviour_Player.animator.applyRootMotion = true;
        characterBehaviour_Player.isRootAnimating = true;
        characterBehaviour_Player.player_Movement.canMove = false;
        characterBehaviour_Player.player_Movement.canRotate = false;
    }
    public void EndRootMotion()
    {
        elapsedTime = 0;
        characterBehaviour_Player.animator.applyRootMotion = false;
        characterBehaviour_Player.isRootAnimating = false;
        characterBehaviour_Player.player_Movement.canMove = true;
        characterBehaviour_Player.player_Movement.canRotate = true;
    }
}