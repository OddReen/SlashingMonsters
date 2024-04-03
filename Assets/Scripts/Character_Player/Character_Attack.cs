using System.Collections;
using UnityEngine;

public class Character_Attack : CharacterActions
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject weaponSlot;
    [SerializeField] public GameObject interactUI;

    [SerializeField] public bool canInteract = false;

    public float damageAmount = 25f;

    public override void Action()
    {
        if (Player_Input.Instance.isAttacking && !characterBehaviour_Player.isRootAnimating)
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
        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();

        yield return new WaitForEndOfFrame();
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
}