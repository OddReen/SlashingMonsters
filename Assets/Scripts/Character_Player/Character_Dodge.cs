using System.Collections;
using UnityEngine;

public class Character_Dodge : CharacterActions
{
    [SerializeField] public float rotationSpeed = 10;
    public override void UpdateAction()
    {
        if (Player_Input.Instance.isDodging && !characterBehaviour_Player.isPerformingAction)
        {
            StartCoroutine(OnAnimation());
        }
    }
    IEnumerator OnAnimation()
    {
        // Direction To Dodge
        float _targetRotation = 0;
        if (characterBehaviour_Player.player_Movement.moveDirectionWorldRelative != Vector3.zero)
            _targetRotation = Mathf.Atan2(characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.x, characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        // Break If No Direction Given
        else
            yield break;

        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();
        yield return new WaitForEndOfFrame();
        while (characterBehaviour_Player.animator.GetNextAnimatorStateInfo(0).IsTag(actionTag) || characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
        {
            float rotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation, rotationSpeed);
            characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
            characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z);
            yield return null;
        }
        EndRootMotion();
    }
}
