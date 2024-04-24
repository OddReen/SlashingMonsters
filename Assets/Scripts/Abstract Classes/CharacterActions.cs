using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterActions : MonoBehaviour
{
    [Header("Action Properties")]
    [SerializeField] protected string actionTag = "";

    protected CharacterBehaviour_Player characterBehaviour_Player;

    protected virtual void Awake()
    {
        characterBehaviour_Player = GetComponent<CharacterBehaviour_Player>();
    }
    public virtual void UpdateAction()
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
        characterBehaviour_Player.isPerformingAction = true;
        characterBehaviour_Player.player_Movement.canMove = false;
        characterBehaviour_Player.player_Movement.canRotate = false;
    }
    public void EndRootMotion()
    {
        characterBehaviour_Player.isPerformingAction = false;
        characterBehaviour_Player.player_Movement.canMove = true;
        characterBehaviour_Player.player_Movement.canRotate = true;
    }
}