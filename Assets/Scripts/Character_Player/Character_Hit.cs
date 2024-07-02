using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Hit : CharacterActions
{
    Coroutine c_OnAnimation;
    public override void UpdateAction()
    {
        if (characterBehaviour_Player.animator.GetNextAnimatorStateInfo(0).IsTag(actionTag) || characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag))
        {
            if (c_OnAnimation == null)
            {
                c_OnAnimation = StartCoroutine(OnAnimation());
            }
        }
    }
    private IEnumerator OnAnimation()
    {
        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();

        yield return new WaitForEndOfFrame();
        while (characterBehaviour_Player.animator.GetNextAnimatorStateInfo(0).IsTag(actionTag) || characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
        {
            characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z);
            yield return null;
        }

        EndRootMotion();
        c_OnAnimation = null;
    }
}
