using System.Collections;
using UnityEngine;

public class Character_Interact : CharacterActions
{
    [SerializeField] GameObject target;

    [SerializeField] private float positionSpeed;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private float targetRotation;
    [SerializeField] private Vector3 targetPosition;

    bool isSomtin;

    public override void UpdateAction()
    {
        SeekInteractables();
        if (Player_Input.Instance.isInteracting && !characterBehaviour_Player.isPerformingAction && characterBehaviour_Player.canInteract)
        {
            if (!isSomtin)
            {
                characterBehaviour_Player.sounds.Swing();
            } 
            isSomtin = true;
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();
            targetPosition = target.transform.position - direction * target.GetComponent<Interactable>().distanceToInteract;
            targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            StartCoroutine(TriggerAnimation());
        }
        else
        {
            isSomtin = false;
        }
    }
    private void SeekInteractables()
    {
        GameObject[] interactables = GameManager.Instance.interactables;
        target = null;

        characterBehaviour_Player.interactUI.SetActive(false);
        characterBehaviour_Player.canInteract = false;

        float minDis = float.PositiveInfinity;

        for (int i = 0; i < interactables.Length; i++)
        {
            if (interactables[i] != null)
            {
                if (interactables[i].GetComponent<Interactable>().canInteract)
                {
                    float currentDis = Vector3.Distance(characterBehaviour_Player.player_CameraController.cameraTarget.transform.position, interactables[i].transform.position);
                    if (currentDis < 2 && currentDis < minDis)
                    {
                        target = interactables[i];
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
    public void Interact()
    {
        target.GetComponent<Interactable>().Action(characterBehaviour_Player);
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
