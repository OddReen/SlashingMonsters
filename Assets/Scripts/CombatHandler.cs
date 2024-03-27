using Cinemachine;
using System.Collections;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerBehaviour playerController;
    Animator animator;
    Rigidbody rb;

    [SerializeField] GameObject weaponSlot;

    [SerializeField] public bool isRootAnimating = false;
    [SerializeField] bool isAttacking = false;
    [SerializeField] public bool isDodging = false;

    public float elapsedTime = 0f;
    public float damageAmount = 25f;

    [SerializeField] string attackTag = "isAttacking";
    [SerializeField] string dodgeTag = "isDodging";

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerController = GetComponent<PlayerBehaviour>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Dodge();
        Attack();
    }
    private void LateUpdate()
    {
        animator.transform.position = transform.position;
    }
    void Attack()
    {
        if (inputHandler.isAttacking && !isRootAnimating) // Begin Attack
        {
            #region Random Attack
            if (animator.GetInteger("RandAttack") >= 2)
            {
                animator.SetInteger("RandAttack", 0);
            }
            else
            {
                animator.SetInteger("RandAttack", animator.GetInteger("RandAttack") + 1);
            }
            #endregion
            StartCoroutine(OnAnimation(attackTag));
        }
    }
    private void Dodge()
    {
        if (inputHandler.isDodging && !isRootAnimating) // Begin Dodge
        {
            StartCoroutine(OnAnimation(dodgeTag));
        }
    }
    IEnumerator OnAnimation(string animationTag)
    {
        animator.SetBool(animationTag, true);
        yield return new WaitForEndOfFrame();
        animator.SetBool(animationTag, false);

        rb.velocity = Vector3.zero;
        playerController.currentSpeed = 0;
        isRootAnimating = true;
        playerController.enabled = false;

        float _targetRotation = 0;
        if (playerController.moveDirectionWorldRelative != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(playerController.moveDirectionWorldRelative.x, playerController.moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        }
        else if (animationTag == dodgeTag)
        {
            yield break;
        }

        while (elapsedTime <= animator.GetCurrentAnimatorStateInfo(0).length * animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && animator.GetCurrentAnimatorStateInfo(0).IsTag(animationTag))
        {
            if (animationTag == dodgeTag)
            {
                float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * playerController.rotationSpeed);

                rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));

                rb.velocity = new Vector3(animator.velocity.x * 1.5f, rb.velocity.y, animator.velocity.z * 1.5f);
            }
            else
            {
                rb.velocity = new Vector3(animator.velocity.x, rb.velocity.y, animator.velocity.z);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0;
        isRootAnimating = false;
        playerController.enabled = true;
        rb.velocity = Vector3.zero;
        playerController.currentSpeed = 0;
    }
}
