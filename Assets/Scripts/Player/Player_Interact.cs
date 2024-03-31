using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Interact : PlayerActions
{
    [SerializeField] private float positionSpeed;
    [SerializeField] private float rotationSpeed;
    public override void Action()
    {
        if (Vector3.Distance(transform.position, GameManager.Instance.checkpoint.transform.position) < 2)
        {
            characterBehaviour_Player.interactUI.SetActive(true);
            characterBehaviour_Player.canInteract = true;
        }
        else
        {
            characterBehaviour_Player.interactUI.SetActive(false);
            characterBehaviour_Player.canInteract = false;
        }
        if (Player_Input.Instance.isInteracting && !characterBehaviour_Player.isRootAnimating && characterBehaviour_Player.canInteract)
        {
            StartCoroutine(OnAnimation());
        }
    }
    IEnumerator OnAnimation()
    {
        Vector3 direction = GameManager.Instance.checkpoint.transform.position - transform.position;
        direction.Normalize();
        Vector3 targetPosition = GameManager.Instance.checkpoint.transform.position - direction;
        float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();
        yield return new WaitForEndOfFrame();
        while (elapsedTime <= characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).length * characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
        {
            float rotation = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, Time.deltaTime * rotationSpeed);

            float positionX = Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * positionSpeed);
            float positionZ = Mathf.Lerp(transform.position.z, targetPosition.z, Time.deltaTime * positionSpeed);

            characterBehaviour_Player.rb.MovePosition(new Vector3(positionX, transform.position.y, positionZ));

            characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        characterBehaviour_Player.healthSystem.Die();
        EndRootMotion();
    }
}
