using System.Collections;
using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    InputHandler inputHandler;
    PlayerBehaviour playerController;
    HealthSystem healthSystem;
    [SerializeField] Animator animator;
    Rigidbody rb;

    [SerializeField] GameObject menu;
    [SerializeField] GameObject weaponSlot;
    [SerializeField] public GameObject interactUI;

    [SerializeField] public bool canInteract = false;
    [SerializeField] public bool isRootAnimating = false;

    public float elapsedTime = 0f;
    public float damageAmount = 25f;

    [SerializeField] string attackTag = "isAttacking";
    [SerializeField] string dodgeTag = "isDodging";
    [SerializeField] string kickTag = "isKicking";
    [SerializeField] string parryTag = "isParrying";
    [SerializeField] string interactTag = "isInteracting";

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        playerController = GetComponent<PlayerBehaviour>();
        rb = GetComponent<Rigidbody>();
        healthSystem = GetComponent<HealthSystem>();
    }
    private void Update()
    {
        Dodge();
        Attack();
        Kick();
        Parry();
        Interact();
        Menu();
    }
    private void LateUpdate()
    {
        animator.transform.position = transform.position;
    }
    private void Attack()
    {
        if (inputHandler.isAttacking && !isRootAnimating)
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
        if (inputHandler.isDodging && !isRootAnimating)
        {
            StartCoroutine(OnAnimation(dodgeTag));
        }
    }
    private void Kick()
    {
        if (inputHandler.isKicking && !isRootAnimating)
        {
            StartCoroutine(OnAnimation(kickTag));
        }
    }
    private void Parry()
    {
        if (inputHandler.isParrying && !isRootAnimating)
        {
            StartCoroutine(OnAnimation(parryTag));
        }
    }
    private void Interact()
    {
        if (Vector3.Distance(transform.position, GameManager.Instance.checkpoint.transform.position) < 2)
        {
            interactUI.SetActive(true);
            canInteract = true;
        }
        else
        {
            interactUI.SetActive(false);
            canInteract = false;
        }
        if (inputHandler.isInteracting && !isRootAnimating && canInteract)
        {
            StartCoroutine(OnAnimation(interactTag));

        }
    }
    private void Menu()
    {
        if (inputHandler.isMenuing)
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);
                Time.timeScale = 1f;
                inputHandler.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                menu.SetActive(true);
                Time.timeScale = 0f;
                inputHandler.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
    IEnumerator OnAnimation(string animationTag)
    {
        float _targetRotation = 0;

        if (playerController.moveDirectionWorldRelative != Vector3.zero)
        {
            _targetRotation = Mathf.Atan2(playerController.moveDirectionWorldRelative.x, playerController.moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        }
        else if (animationTag == dodgeTag)
        {
            yield break;
        }

        animator.SetBool(animationTag, true);
        yield return new WaitForEndOfFrame();
        animator.SetBool(animationTag, false);

        rb.velocity = Vector3.zero;
        playerController.currentSpeed = 0;
        isRootAnimating = true;
        playerController.enabled = false;

        Vector3 direction = GameManager.Instance.checkpoint.transform.position - transform.position;
        direction.Normalize();
        Vector3 targetPosition = GameManager.Instance.checkpoint.transform.position - direction;
        float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        while (elapsedTime <= animator.GetCurrentAnimatorStateInfo(0).length * animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && animator.GetCurrentAnimatorStateInfo(0).IsTag(animationTag) && !healthSystem.isDead)
        {
            if (animationTag == dodgeTag)
            {
                float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * playerController.rotationSpeed);

                rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));

                rb.velocity = new Vector3(animator.velocity.x * 1.5f, rb.velocity.y, animator.velocity.z * 1.5f);
            }
            else if (animationTag == interactTag)
            {
                float rotation = Mathf.LerpAngle(transform.eulerAngles.y, targetRotation, Time.deltaTime * playerController.rotationSpeed);
                float positionX = Mathf.Lerp(transform.position.x, targetPosition.x, Time.deltaTime * playerController.rotationSpeed);
                float positionZ = Mathf.Lerp(transform.position.z, targetPosition.z, Time.deltaTime * playerController.rotationSpeed);

                rb.MovePosition(new Vector3(positionX, transform.position.y, positionZ));
                rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
            }
            else
            {
                rb.velocity = new Vector3(animator.velocity.x, rb.velocity.y, animator.velocity.z);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (animationTag == interactTag)
        {
            healthSystem.Die();
        }
        elapsedTime = 0;
        rb.velocity = Vector3.zero;
        playerController.currentSpeed = 0;
        isRootAnimating = false;
        playerController.enabled = true;
    }
}