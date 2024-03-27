using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerControls inputActions;

    public Vector2 movementInput;
    public Vector2 cameraInput;
    public bool isRunning;
    public bool isAiming;
    public bool isAttacking;
    public bool isDodging;

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
            //inputActions.Gameplay.Aim.performed += context => isAiming = !isAiming;
            inputActions.Gameplay.Aim.performed += context => StartAim();
            //inputActions.Gameplay.Attack.performed += context => isAttacking = context.ReadValueAsButton();
            //inputActions.Gameplay.Attack.canceled += context => isAttacking = context.ReadValueAsButton();
            inputActions.Gameplay.Attack.performed += context => StartAttack();

            //inputActions.Gameplay.Dodge.performed += context => isDodging = context.ReadValueAsButton();
            //inputActions.Gameplay.Dodge.canceled += context => isDodging = context.ReadValueAsButton();
            inputActions.Gameplay.Dodge.performed += context => StartDodge();
        }
        inputActions.Enable();
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