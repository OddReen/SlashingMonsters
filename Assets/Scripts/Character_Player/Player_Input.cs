using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Player_Input : MonoBehaviour
{
    public static Player_Input Instance;
    public PlayerControls inputActions;

    public bool isInGameMenu = false;
    [SerializeField] float cameraMouseSensivity = 1f;
    [SerializeField] float cameraControllerSensivity = 2f;

    public Vector2 movementInput;
    public Vector2 cameraInput;
    public bool isRunning;
    public bool isThrowing;
    public bool isAttacking;
    public bool isDodging;
    public bool isKicking;
    public bool isParrying;
    public bool isInteracting;
    public bool isMenuing;

    Coroutine c_Interact;
    Coroutine c_Kick;
    Coroutine c_Parry;
    Coroutine c_Attack;
    Coroutine c_Dodge;
    Coroutine c_Throw;

    [SerializeField] float triggerTime = .5f;

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

            inputActions.Gameplay.Camera.performed += context =>
            {
                if (context.control.device is Mouse)
                cameraInput = context.ReadValue<Vector2>() * cameraMouseSensivity;
                else
                {
                    cameraInput = context.ReadValue<Vector2>() * cameraControllerSensivity;
                }
            };
            inputActions.Gameplay.Camera.canceled += context => cameraInput = context.ReadValue<Vector2>();

            inputActions.Gameplay.Run.performed += context => isRunning = context.ReadValueAsButton();
            inputActions.Gameplay.Run.canceled += context => isRunning = context.ReadValueAsButton();

            inputActions.Gameplay.Throw.performed += context => StartThrow();

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
        if (isInGameMenu) return;
        if (c_Interact != null)
            StopCoroutine(c_Interact);
        c_Interact = StartCoroutine(TriggerInteract());
    }
    void StartKick()
    {
        if (isInGameMenu) return;
        if (c_Kick != null)
            StopCoroutine(c_Kick);
        c_Kick = StartCoroutine(TriggerKick());
    }
    void StartParry()
    {
        if (isInGameMenu) return;
        if (c_Parry != null)
            StopCoroutine(c_Parry);
        c_Parry = StartCoroutine(TriggerParry());
    }
    void StartAttack()
    {
        if (isInGameMenu) return;
        if (c_Attack != null)
            StopCoroutine(c_Attack);
        c_Attack = StartCoroutine(TriggerAttack());

    }
    void StartDodge()
    {
        if (isInGameMenu) return;
        if (c_Dodge != null)
            StopCoroutine(c_Dodge);
        c_Dodge = StartCoroutine(TriggerDodge());
    }
    void StartThrow()
    {
        if (isInGameMenu) return;
        if (c_Throw != null)
            StopCoroutine(c_Throw);
        c_Throw = StartCoroutine(TriggerThrow());
    }

    IEnumerator TriggerMenu()
    {
        isMenuing = true;
        yield return new WaitForEndOfFrame();
        isMenuing = false;
    }
    IEnumerator TriggerThrow()
    {
        isThrowing = true;
        yield return new WaitForEndOfFrame();
        isThrowing = false;
    }
    IEnumerator TriggerInteract()
    {
        isInteracting = true;
        yield return new WaitForSeconds(triggerTime);
        isInteracting = false;
    }
    IEnumerator TriggerKick()
    {
        isKicking = true;
        yield return new WaitForSeconds(triggerTime);
        isKicking = false;
    }
    IEnumerator TriggerParry()
    {
        isParrying = true;
        yield return new WaitForSeconds(triggerTime);
        isParrying = false;
    }
    IEnumerator TriggerAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(triggerTime);
        isAttacking = false;
    }
    IEnumerator TriggerDodge()
    {
        isDodging = true;
        yield return new WaitForSeconds(triggerTime);
        isDodging = false;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}