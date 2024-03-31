using Cinemachine;
using UnityEngine;

public class Player_TargetAim : PlayerActions
{
    Rigidbody rb;
    Player_Attack player_Attack;
    Player_Input inputHandler;
    Player_Movement playerController;
    Player_CameraController cameraController;
    [SerializeField] CinemachineVirtualCamera AimCamera;

    [SerializeField] GameObject cameraTarget;
    [SerializeField] GameObject target;
    [SerializeField] float aimRadius;
    [SerializeField] float rotationSpeed = 20;
    [SerializeField] LayerMask enemiesLayerMask;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] bool isAiming = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player_Attack = GetComponent<Player_Attack>();
        inputHandler = GetComponent<Player_Input>();
        cameraController = GetComponent<Player_CameraController>();
        playerController = GetComponent<Player_Movement>();
    }
    private void FixedUpdate()
    {
        if (isAiming && target != null && !characterBehaviour_Player.isRootAnimating) // On Aiming
        {
            RotateTowardsTarget();
        }
    }
    private void LateUpdate()
    {
        if (inputHandler.isAiming)
        {
            isAiming = !isAiming;
        }
        if (isAiming && target == null) //On Aim
        {
            OnAim();
        }
        else if (!isAiming && target != null)  //On Stop Aim
        {
            OnStopAim();
        }
    }
    private void RotateTowardsTarget()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        targetDir.Normalize();
        float _targetRotation = Mathf.Atan2(targetDir.x, targetDir.z) * Mathf.Rad2Deg;
        float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeed);

        rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
    }
    void OnAim()
    {
        TargetToAim();
        if (target != null)
        {
            AimCamera.m_LookAt = target.transform;
            isAiming = true;
        }
        else
        {
            isAiming = false;
            return;
        }

        Vector3 dir = target.transform.position - cameraTarget.transform.position;
        dir.Normalize();
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);

        cameraController.cameraTargetPitch = rotation.eulerAngles.x;
        cameraController.cameraTargetYaw = rotation.eulerAngles.y;

        playerController.canRotate = false;
        cameraController.enabled = false;
        AimCamera.gameObject.SetActive(true);
    }
    void OnStopAim()
    {
        Vector3 dir = target.transform.position - cameraTarget.transform.position;
        dir.Normalize();
        Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);

        cameraController.cameraTargetPitch = rotation.eulerAngles.x;
        cameraController.cameraTargetYaw = rotation.eulerAngles.y;

        playerController.canRotate = true;
        cameraController.enabled = true;
        AimCamera.gameObject.SetActive(false);

        target = null;
    }
    private void TargetToAim()
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, aimRadius, enemiesLayerMask); //Grab every enemy in area around player

        float minDot = 0.5f;

        for (int i = 0; i < collider.Length; i++)
        {
            Vector3 dir = (collider[i].transform.position - cameraTarget.transform.position);
            dir.Normalize();

            float currentDot = Vector3.Dot(cameraTarget.transform.forward, dir);

            if (currentDot > 0.5f)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(cameraTarget.transform.position, dir, out raycastHit, aimRadius, ~playerLayerMask))
                {
                    if (raycastHit.collider.CompareTag("Enemy"))
                    {
                        if (currentDot > minDot)
                        {
                            target = raycastHit.collider.gameObject;
                            minDot = currentDot;
                        }
                        else if (minDot == 0.5f)
                        {
                            target = raycastHit.collider.gameObject;
                        }
                    }
                }
            }
        }
    }
}
