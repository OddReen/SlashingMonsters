using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Parry : CharacterActions
{
    public override void Action()
    {
        if (Player_Input.Instance.isParrying && !characterBehaviour_Player.isRootAnimating)
        {
            StartCoroutine(OnAnimation());
        }
    }
    private IEnumerator OnAnimation()
    {
        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();

        yield return new WaitForEndOfFrame();
        StartCoroutine(ParryWindow());
        if (characterBehaviour_Player.player_Movement.moveDirectionWorldRelative != Vector3.zero)
        {
            float _targetRotation = 0;
            _targetRotation = Mathf.Atan2(characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.x, characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            while (elapsedTime <= characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).length * characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
            {
                float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * characterBehaviour_Player.player_Movement.rotationSpeed);
                characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
                characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (elapsedTime <= characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).length * characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
            {
                characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        EndRootMotion();
    }

    private IEnumerator ParryWindow()
    {
        characterBehaviour_Player.isParrying = true;
        yield return new WaitForSeconds(characterBehaviour_Player.parryWindow);
        characterBehaviour_Player.isParrying = false;
    }
}
