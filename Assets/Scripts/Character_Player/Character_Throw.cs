using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Character_Throw : CharacterActions
{
    [SerializeField] CinemachineVirtualCamera AimCamera;

    [SerializeField] GameObject highlightPref;
    [SerializeField] GameObject highlight;
    [SerializeField] GameObject target;
    [SerializeField] float aimRadius;
    [SerializeField] float rotationSpeed = 20;

    private void Start()
    {
        highlight = Instantiate(highlightPref);
        highlight.name = highlightPref.name;
        highlight.SetActive(false);
        highlight.transform.SetParent(transform);
    }
    public override void UpdateAction()
    {
        if (characterBehaviour_Player.hasThrowable)
        {
            TargetEnemy();
            if (Player_Input.Instance.isThrowing && target != null)
            {
                StartCoroutine(OnAnimation());
            }
        }
    }
    private IEnumerator OnAnimation()
    {
        StartCoroutine(TriggerAnimation());
        InitializeRootMotion();

        yield return new WaitForEndOfFrame();
        float _targetRotation = 0;
        _targetRotation = Mathf.Atan2(characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.x, characterBehaviour_Player.player_Movement.moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        while (characterBehaviour_Player.animator.GetCurrentAnimatorStateInfo(0).IsTag(actionTag) && !characterBehaviour_Player.isDead)
        {
            RotateTowardsTarget();
            characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.animator.velocity.x, characterBehaviour_Player.rb.velocity.y, characterBehaviour_Player.animator.velocity.z);
            yield return null;
        }

        EndRootMotion();
    }
    public void Throw(Transform throwableSlot)
    {
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            dir.Normalize();
            
            Transform throwable = throwableSlot.GetChild(0);

            throwable.GetComponent<Rigidbody>().isKinematic = false;
            throwable.GetComponent<Rigidbody>().AddForce(dir * Vector3.Distance(target.transform.position, transform.position));

            throwable.GetComponent<HitBox>().EnableHitBox();


            throwableSlot.DetachChildren();
        }
    }
    private void RotateTowardsTarget()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        float _targetRotation = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeed);

        characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
    }
    private void TargetEnemy()
    {
        target = null;

        Collider[] collider = Physics.OverlapSphere(characterBehaviour_Player.player_CameraController.cameraTarget.transform.position, aimRadius, characterBehaviour_Player.enemyMask);
        if (collider.Length == 0)
            return;

        float minDis = float.PositiveInfinity;

        for (int i = 0; i < collider.Length; i++)
        {
            float currentDis = Vector3.Distance(characterBehaviour_Player.player_CameraController.cameraTarget.transform.position, collider[i].transform.position);

            Vector3 dir = collider[i].transform.position - characterBehaviour_Player.player_CameraController.cameraTarget.transform.position;
            dir.Normalize();

            RaycastHit raycastHit;
            if (Physics.Raycast(characterBehaviour_Player.player_CameraController.cameraTarget.transform.position, dir, out raycastHit, aimRadius, ~characterBehaviour_Player.playerMask))
            {
                if (raycastHit.collider.CompareTag("Enemy"))
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
            highlight.SetActive(true);
            highlight.transform.position = target.transform.position + Vector3.up * 2;
        }
        else
        {
            highlight.SetActive(false);
            return;
        }
    }
}
