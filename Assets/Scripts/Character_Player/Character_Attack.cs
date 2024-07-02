using System.Collections;
using UnityEngine;

public class Character_Attack : CharacterActions
{
    [SerializeField] GameObject menu;
    [SerializeField] public GameObject interactUI;

    [SerializeField] public bool canInteract = false;

    public float rotationSpeedOnPrepare = 5f;

    Coroutine c_OnAnimation;

    public override void UpdateAction()
    {
        if (Player_Input.Instance.isAttacking && characterBehaviour_Player.hasWeapon)
        {
            if (!characterBehaviour_Player.isPerformingAction)
            {
                c_OnAnimation = StartCoroutine(OnAnimation());
            }
            if (characterBehaviour_Player.atackingState == CharacterBehaviour.AtackingState.Ending)
            {
                c_OnAnimation = StartCoroutine(OnAnimation());
            }
        }
    }
    private IEnumerator OnAnimation()
    {
        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();

        characterBehaviour_Player.atackingState = CharacterBehaviour.AtackingState.Preparing;

        yield return new WaitForEndOfFrame();

        float _targetRotation = Mathf.Atan2(characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.x, characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y; 

        while (characterBehaviour_Player.animator.GetNextAnimatorStateInfo(0).IsTag(actionTag) || characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
        {
            if (Player_Input.Instance.isAttacking && characterBehaviour_Player.atackingState == CharacterBehaviour.AtackingState.Ending) 
                break; 
            if (characterBehaviour_Player.player_Movement.moveDirectionWorldRelative != Vector3.zero && characterBehaviour_Player.atackingState == CharacterBehaviour.AtackingState.Preparing)
            {
                float rotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeedOnPrepare);
                characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
            }
            characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z);
            yield return null;
        }

        EndRootMotion();

        characterBehaviour_Player.atackingState = CharacterBehaviour.AtackingState.None;
    }
}