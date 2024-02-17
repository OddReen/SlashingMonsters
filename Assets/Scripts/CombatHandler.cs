using Cinemachine;
using System.Collections;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    InputHandler inputHandler;
    CameraController cameraController;
    PlayerController playerController;
    Animator animator;
    Rigidbody rb;
    HealthSystem healthSystem;
    [SerializeField] GameObject weaponSlot;
    Collider weaponCollider;

    bool hasAimed = false;
    [SerializeField] bool hasAttacked = false;
    [SerializeField] bool isAttacking = false;
    [SerializeField] CinemachineVirtualCamera AimCamera;

    [SerializeField] GameObject cameraTarget;
    [SerializeField] GameObject target;

    [SerializeField] float rotationSpeed = 20;
    public float elapsedTime = 0f;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        cameraController = GetComponent<CameraController>();
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        healthSystem = GetComponent<HealthSystem>();
        weaponCollider = weaponSlot.GetComponent<Collider>();
        weaponCollider.enabled = false;
    }
    private void FixedUpdate()
    {
        if (isAttacking)
        {
            transform.position = animator.gameObject.transform.position;
            Vector3 velocity = new Vector3(animator.velocity.x, rb.velocity.y, animator.velocity.z);
            rb.velocity = velocity;
        }
        Attack();
    }
    private void LateUpdate()
    {
        if (inputHandler.isAiming) // On Aiming
        {
            Rotation();
        }
        if (inputHandler.isAiming && !hasAimed) //On Aim
        {
            hasAimed = true;
            OnAim();
        }
        else if (!inputHandler.isAiming && hasAimed)  //On Stop Aim
        {
            hasAimed = false;
            OnStopAim();
        }
        animator.gameObject.transform.position = transform.position;
    }
    private void Rotation()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        float _targetRotation = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeed);

        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
    }
    void Attack()
    {
        if (inputHandler.isAttacking && !hasAttacked && !isAttacking) // Begin Attack
        {
            hasAttacked = true;
            animator.SetInteger("RandAttack", Random.Range(0, 7));
            animator.SetBool("isAttacking", true);
            StartCoroutine(HasAttaked());
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && hasAttacked && !isAttacking)  //If is Attacking
        {
            hasAttacked = false;
            isAttacking = true;
            playerController.enabled = false;
            StartCoroutine(OnAnimationEnd());
        }
    }
    IEnumerator HasAttaked()
    {
        yield return new WaitForEndOfFrame();
        animator.SetBool("isAttacking", false);
    }
    IEnumerator OnAnimationEnd()
    {
        while (elapsedTime <= animator.GetCurrentAnimatorStateInfo(0).length - animator.GetAnimatorTransitionInfo(0).duration * animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        playerController.enabled = true;
        elapsedTime = 0;
        isAttacking = false;
    }
    void OnAim()
    {
        playerController.canRotate = false;

        cameraController.enabled = false;
        AimCamera.gameObject.SetActive(true);
    }
    void OnStopAim()
    {
        playerController.canRotate = true;
        cameraController.enabled = true;

        AimCamera.gameObject.SetActive(false);

        Vector3 dir = target.transform.position - cameraTarget.transform.position;
        dir.Normalize();
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);

        cameraController.cameraTargetPitch = rotation.eulerAngles.x;
        cameraController.cameraTargetYaw = rotation.eulerAngles.y;
    }
    private void OnDrawGizmos()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + targetDir);
    }
}
