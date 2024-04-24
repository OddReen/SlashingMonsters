using System.Collections;
using UnityEngine;

public class Character_Interact : CharacterActions
{
    [SerializeField] GameObject target;

    [SerializeField] private float positionSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float targetRotation;
    [SerializeField] private Vector3 targetPosition;
    public override void UpdateAction()
    {
        TargetInteractable();
        if (Player_Input.Instance.isInteracting && !characterBehaviour_Player.isPerformingAction && characterBehaviour_Player.canInteract)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();
            targetPosition = target.transform.position - direction * 0.5f;
            targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            StartCoroutine(OnAnimation());
        }
    }
    private void TargetInteractable()
    {
        target = null;

        characterBehaviour_Player.interactUI.SetActive(false);
        characterBehaviour_Player.canInteract = false;

        Collider[] collider = Physics.OverlapSphere(characterBehaviour_Player.player_CameraController.cameraTarget.transform.position, characterBehaviour_Player.interactRadius, characterBehaviour_Player.interactableMask);
        if (collider.Length == 0)
            return;

        float minDis = float.PositiveInfinity;

        for (int i = 0; i < collider.Length; i++)
        {
            float currentDis = Vector3.Distance(characterBehaviour_Player.player_CameraController.cameraTarget.transform.position, collider[i].transform.position);

            Vector3 dir = collider[i].transform.position - characterBehaviour_Player.player_CameraController.cameraTarget.transform.position;
            dir.Normalize();

            RaycastHit raycastHit;
            if (Physics.Raycast(characterBehaviour_Player.player_CameraController.cameraTarget.transform.position, dir, out raycastHit, characterBehaviour_Player.interactRadius, ~characterBehaviour_Player.playerMask))
            {
                if (raycastHit.collider.CompareTag("Interactable"))
                {
                    if (currentDis < minDis)
                    {
                        target = raycastHit.collider.gameObject;
                        minDis = currentDis;
                    }
                }
            }
        }
        if (target != null)
        {
            characterBehaviour_Player.interactUI.SetActive(true);
            characterBehaviour_Player.canInteract = true;
        }
    }
    IEnumerator OnAnimation()
    {
        GameObject currentTarget = target;
        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();
        yield return new WaitForEndOfFrame();
        while (characterBehaviour_Player.animator.GetNextAnimatorStateInfo(0).IsTag(actionTag) || characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
        {
            float rotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, Time.deltaTime * rotationSpeed);

            float positionX = Mathf.MoveTowards(transform.position.x, targetPosition.x, Time.deltaTime * positionSpeed);
            float positionZ = Mathf.MoveTowards(transform.position.z, targetPosition.z, Time.deltaTime * positionSpeed);

            characterBehaviour_Player.rb.MovePosition(new Vector3(positionX, transform.position.y, positionZ));

            characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
            yield return null;
        }
        EndRootMotion();
        currentTarget.GetComponent<Interactable>().Action(characterBehaviour_Player);
    }
    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + Vector3.up, target.transform.position);
            Gizmos.DrawSphere(target.transform.position, .25f);
        }
    }
}
