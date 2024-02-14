using UnityEngine;

public class PlayerController : MonoBehaviour
{
    InputHandler inputHandler;

    Rigidbody _rigidbody;
    Animator _animator;

    [Header("Stats")]
    [SerializeField] float currentSpeed;
    [SerializeField] float walkingSpeed = 5;
    [SerializeField] float runningSpeed = 8;
    [SerializeField] float blendSpeed;
    [SerializeField] float rotationSpeed = 10;

    [Header("Direction")]
    public Vector3 moveDirectionWorldRelative;
    public Vector3 moveDirectionCameraRelative;

    [Header("IsGrounded")]
    [SerializeField] bool isGrounded;
    [SerializeField] float sphereOverlapHeight;
    [SerializeField] float sphereOverlapRadius;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float rayDistanceDown;
    [SerializeField] Vector3 rayHit;

    [Header("Gizmos")]
    [SerializeField] bool playerDirectionGizmo;
    [SerializeField] bool isGroundedGizmo;

    [Header("Bolleans")]
    [SerializeField] public bool canRotate = true;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        inputHandler = GetComponent<InputHandler>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void FixedUpdate()
    {
        IsGrounded();
        Movement();
        Rotation();
        StickToTheGround();
    }
    private void LateUpdate()
    {
        ChangeModelPos();
    }
    private void Movement()
    {
        if (!isGrounded)
            return;
        moveDirectionWorldRelative = new Vector3(inputHandler.movementInput.x, 0, inputHandler.movementInput.y);

        moveDirectionCameraRelative = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized * moveDirectionWorldRelative.z) + (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized * moveDirectionWorldRelative.x);
        moveDirectionCameraRelative.Normalize();

        float targetSpeed = inputHandler.isRunning ? runningSpeed : walkingSpeed;

        if (inputHandler.isRunning && inputHandler.movementInput.magnitude > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * blendSpeed);
            _animator.SetFloat("Move", currentSpeed);
        }
        else if (inputHandler.movementInput.magnitude > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * blendSpeed);
            _animator.SetFloat("Move", currentSpeed);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime * blendSpeed);
            _animator.SetFloat("Move", currentSpeed);
        }
        Vector3 moveVelocity = moveDirectionCameraRelative * targetSpeed;

        moveVelocity.y = _rigidbody.velocity.y;

        _rigidbody.velocity = moveVelocity;
    }
    private void Rotation()
    {
        if (moveDirectionWorldRelative != Vector3.zero && canRotate)
        {
            float _targetRotation = Mathf.Atan2(moveDirectionWorldRelative.x, moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeed);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }
    private bool IsGrounded()
    {
        if (Physics.CheckSphere(transform.position + Vector3.up * sphereOverlapHeight, sphereOverlapRadius, ~layerMask))
        {
            isGrounded = true;
            return isGrounded;
        }
        else
        {
            isGrounded = false;
            return isGrounded;
        }
    }
    bool triggerOffTheGround = false;
    private void StickToTheGround()
    {
        if (isGrounded)
        {
            triggerOffTheGround = false;
            _animator.SetBool("isGrounded", true);
        }
        if (isGrounded && moveDirectionWorldRelative.magnitude <= .1f)
        {
            _rigidbody.velocity = Vector3.zero;
        }
        if (!isGrounded)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistanceDown, ~layerMask) && !triggerOffTheGround)
            {
                _animator.SetBool("isGrounded", true);
                rayHit = hit.point;
                Vector3 withZeroYVelocity = new Vector3(_rigidbody.velocity.x, -100, _rigidbody.velocity.z);
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, -hit.point.y * 5, _rigidbody.velocity.z);
            }
            else
            {
                _animator.SetBool("isGrounded", false);
            }
            triggerOffTheGround = true;
        }
    }
    private void ChangeModelPos()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistanceDown, ~layerMask) && !triggerOffTheGround)
        {
            rayHit = hit.point;
            Vector3 vector3 = transform.position;
            vector3.y = rayHit.y;
            _animator.gameObject.transform.position = Vector3.Lerp(_animator.gameObject.transform.position, vector3, Time.deltaTime * 25);
        }
        else
        {
            _animator.gameObject.transform.position = transform.position;
        }
    }
    private void OnDrawGizmos()
    {
        if (playerDirectionGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + Vector3.up, transform.position + moveDirectionCameraRelative + Vector3.up);
            Gizmos.DrawWireSphere(transform.position + moveDirectionCameraRelative + Vector3.up, .25f);
        }
        if (isGroundedGizmo)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * sphereOverlapHeight, sphereOverlapRadius);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistanceDown);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistanceDown, ~layerMask))
        {
            Gizmos.DrawLine(hit.point, hit.point + hit.normal);
        }
    }
}