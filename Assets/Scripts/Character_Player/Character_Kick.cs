using System.Collections;
using UnityEngine;

public class Character_Kick : CharacterActions
{
    [SerializeField] float rotationSpeed = 20;
    public override void UpdateAction()
    {
        if (Player_Input.Instance.isKicking && !characterBehaviour_Player.isPerformingAction)
        {
            StartCoroutine(OnAnimation());
        }
    }
    private IEnumerator OnAnimation()
    {
        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();

        yield return new WaitForEndOfFrame();
        float _targetRotation = Mathf.Atan2(characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.x, characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        while (characterBehaviour_Player.animator.GetNextAnimatorStateInfo(0).IsTag(actionTag) || characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
        {
            if (characterBehaviour_Player.player_Movement.moveDirectionWorldRelative != Vector3.zero)
            {
                float rotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeed);
                characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
            }
            characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z);
            yield return null;
        }

        EndRootMotion();
    }
}