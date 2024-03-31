using Cinemachine;
using UnityEngine;
public class Player_CameraController : PlayerActions
{
    public static Player_CameraController Instance;

    [SerializeField] private float globalShakeForce = 1f;

    [SerializeField] GameObject cameraTarget;
    [SerializeField] Transform cameraTargetPosition;
    [SerializeField] CinemachineVirtualCamera thirdPersonCamera;
    [SerializeField] CinemachineVirtualCamera thirdPersonCameraAim;

    public float rotationSpeed = 50f;

    public float topClamp = 85f;
    public float bottomClamp = -85f;

    const float threshold = 0.01f;

    public float cameraTargetPitch;
    public float cameraTargetYaw;

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }

    protected override void Awake()
    {
        Instance = this;
        Cursor.lockState = CursorLockMode.Locked;
        GameObject newCameraTarget = GameObject.Find("Camera Target");
        if (newCameraTarget == null)
        {
            newCameraTarget = new GameObject("Camera Target");
            cameraTarget = newCameraTarget;
        }
        else
        {
            cameraTarget = newCameraTarget;
        }
        thirdPersonCamera.Follow = cameraTarget.transform;
        thirdPersonCamera.LookAt = cameraTarget.transform;
        thirdPersonCameraAim.Follow = cameraTarget.transform;

        cameraTarget.transform.forward = transform.forward;
        cameraTarget.transform.right = transform.right;

        cameraTargetPitch = cameraTarget.transform.rotation.eulerAngles.x;
        cameraTargetYaw = cameraTarget.transform.rotation.eulerAngles.y;

        cameraTarget.transform.position = cameraTargetPosition.position;
    }
    public override void Action()
    {
        cameraTarget.transform.position = cameraTargetPosition.position;
        CameraRotation();
    }
    private void CameraRotation()
    {
        if (Player_Input.Instance.cameraInput.sqrMagnitude >= threshold)
        {
            cameraTargetPitch += -Player_Input.Instance.cameraInput.y * rotationSpeed * Time.deltaTime;
            cameraTargetYaw += Player_Input.Instance.cameraInput.x * rotationSpeed * Time.deltaTime;
        }

        cameraTargetPitch = ClampAngle(cameraTargetPitch, bottomClamp, topClamp); //Clamp Pitch

        cameraTarget.transform.rotation = Quaternion.Euler(cameraTargetPitch, cameraTargetYaw, 0.0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
