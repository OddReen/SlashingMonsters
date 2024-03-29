using System.Collections;
using UnityEngine;

public class Player_Dodge : PlayerActions
{
    public override void Action()
    {
        if (Player_Input.Instance.isDodging && !characterBehaviour_Player.isRootAnimating)
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

        // Activate Animation
        characterBehaviour_Player.animator.SetBool(actionTag, true);
        yield return new WaitForEndOfFrame();
        characterBehaviour_Player.animator.SetBool(actionTag, false);

        // Activate Root Animation
        characterBehaviour_Player.rb.velocity = Vector3.zero;
        characterBehaviour_Player.player_Movement.currentSpeed = 0;
        characterBehaviour_Player.isRootAnimating = true;
        characterBehaviour_Player.player_Movement.enabled = false;

        // Update While Action
        while (elapsedTime <= characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).length * characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.healthSystem.isDead)
        {
            float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * characterBehaviour_Player.player_Movement.rotationSpeed);
            characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
            characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x * 1.5f, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z * 1.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Deactivate Root Animation
        elapsedTime = 0;
        characterBehaviour_Player.rb.velocity = Vector3.zero;
        characterBehaviour_Player.player_Movement.currentSpeed = 0;
        characterBehaviour_Player.isRootAnimating = false;
        characterBehaviour_Player.player_Movement.enabled = true;
    }
}
