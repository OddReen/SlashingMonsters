using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Kick : PlayerActions
{
    public override void Action()
    {
        if (Player_Input.Instance.isKicking && !characterBehaviour_Player.isRootAnimating)
        {
            StartCoroutine(OnAnimation());
        }
    }
    private IEnumerator OnAnimation()
    {
        characterBehaviour_Player.animator.SetBool(actionTag, true);
        yield return new WaitForEndOfFrame();
        characterBehaviour_Player.animator.SetBool(actionTag, false);

        characterBehaviour_Player.rb.velocity = Vector3.zero;
        characterBehaviour_Player.player_Movement.currentSpeed = 0;
        characterBehaviour_Player.isRootAnimating = true;
        characterBehaviour_Player.player_Movement.enabled = false;

        while (elapsedTime <= characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).length * characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.healthSystem.isDead)
        {
            characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        elapsedTime = 0;
        characterBehaviour_Player.rb.velocity = Vector3.zero;
        characterBehaviour_Player.player_Movement.currentSpeed = 0;
        characterBehaviour_Player.isRootAnimating = false;
        characterBehaviour_Player.player_Movement.enabled = true;
    }
}
