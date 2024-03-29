using System.Collections;
using UnityEngine;

public class Player_Attack : PlayerActions
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject weaponSlot;
    [SerializeField] public GameObject interactUI;

    [SerializeField] public bool canInteract = false;
    [SerializeField] public bool isRootAnimating = false;

    public float damageAmount = 25f;

    public override void Action()
    {
        if (Player_Input.Instance.isAttacking && !isRootAnimating)
        {
            #region Random Attack
            if (characterBehaviour_Player.animator.GetInteger("RandAttack") >= 2)
            {
                characterBehaviour_Player.animator.SetInteger("RandAttack", 0);
            }
            else
            {
                characterBehaviour_Player.animator.SetInteger("RandAttack", characterBehaviour_Player.animator.GetInteger("RandAttack") + 1);
            }
            #endregion
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
        isRootAnimating = true;
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
        isRootAnimating = false;
        characterBehaviour_Player.player_Movement.enabled = true;
    }
}