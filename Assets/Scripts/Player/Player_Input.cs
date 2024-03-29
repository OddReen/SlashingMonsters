using System.Collections;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    public PlayerControls inputActions;

    public static Player_Input Instance;

    public Vector2 movementInput;
    public Vector2 cameraInput;
    public bool isRunning;
    public bool isAiming;
    public bool isAttacking;
    public bool isDodging;
    public bool isKicking;
    public bool isParrying;
    public bool isInteracting;
    public bool isMenuing;

    private void Start()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.Gameplay.Movement.performed += context => movementInput = context.ReadValue<Vector2>();
            inputActions.Gameplay.Movement.canceled += context => movementInput = context.ReadValue<Vector2>();

            inputActions.Gameplay.Camera.performed += context => cameraInput = context.ReadValue<Vector2>();
            inputActions.Gameplay.Camera.canceled += context => cameraInput = context.ReadValue<Vector2>();

            inputActions.Gameplay.Run.performed += context => isRunning = context.ReadValueAsButton();
            inputActions.Gameplay.Run.canceled += context => isRunning = context.ReadValueAsButton();

            inputActions.Gameplay.Aim.performed += context => StartAim();

            inputActions.Gameplay.Attack.performed += context => StartAttack();

            inputActions.Gameplay.Dodge.performed += context => StartDodge();

            inputActions.Gameplay.Kick.performed += context => StartKick();

            inputActions.Gameplay.Parry.performed += context => StartParry();

            inputActions.Gameplay.Interact.performed += context => StartInteract();

            inputActions.Gameplay.Menu.performed += context => StartMenu();
        }
        inputActions.Enable();
    }

    void StartMenu()
    {
        StartCoroutine(TriggerMenu());
    }
    void StartInteract()
    {
        StartCoroutine(TriggerInteract());
    }
    void StartKick()
    {
        StartCoroutine(TriggerKick());
    }
    void StartParry()
    {
        StartCoroutine(TriggerParry());
    }
    void StartAttack()
    {
        StartCoroutine(TriggerAttack());
    }
    void StartDodge()
    {
        StartCoroutine(TriggerDodge());
    }
    void StartAim()
    {
        StartCoroutine(TriggerAim());
    }

    IEnumerator TriggerMenu()
    {
        isMenuing = true;
        yield return new WaitForEndOfFrame();
        isMenuing = false;
    }
    IEnumerator TriggerInteract()
    {
        isInteracting = true;
        yield return new WaitForEndOfFrame();
        isInteracting = false;
    }
    IEnumerator TriggerKick()
    {
        isKicking = true;
        yield return new WaitForEndOfFrame();
        isKicking = false;
    }
    IEnumerator TriggerParry()
    {
        isParrying = true;
        yield return new WaitForEndOfFrame();
        isParrying = false;
    }
    IEnumerator TriggerAttack()
    {
        isAttacking = true;
        yield return new WaitForEndOfFrame();
        isAttacking = false;
    }
    IEnumerator TriggerDodge()
    {
        isDodging = true;
        yield return new WaitForEndOfFrame();
        isDodging = false;
    }
    IEnumerator TriggerAim()
    {
        isAiming = true;
        yield return new WaitForEndOfFrame();
        isAiming = false;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}