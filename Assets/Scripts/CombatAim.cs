using Cinemachine;
using UnityEngine;

public class CombatAim : MonoBehaviour
{
    InputHandler inputHandler;
    CameraController cameraController;
    PlayerController playerController;
    bool hasAimed = false;
    [SerializeField] CinemachineVirtualCamera AimCamera;

    [SerializeField] GameObject cameraTarget;
    [SerializeField] GameObject target;

    [SerializeField] float rotationSpeed = 20;

    private void Start()
    {
        inputHandler = GetComponent<InputHandler>();
        cameraController = GetComponent<CameraController>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
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
    }
    private void Rotation()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        float _targetRotation = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeed);

        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
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
        cameraTarget.transform.LookAt(target.transform.position, Vector3.up);
        cameraController.cameraTargetPitch = cameraTarget.transform.rotation.x;
        cameraController.cameraTargetYaw = cameraTarget.transform.rotation.y;
        cameraController.enabled = true;
        AimCamera.gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + targetDir);
    }
}
