using UnityEngine;

public class Character_Movement : CharacterActions
{
    [SerializeField] public bool canMove = true;

    [Header("Stats")]
    public float currentSpeed;
    [SerializeField] public  float walkingSpeed = 5;
    [SerializeField] public  float runningSpeed = 8;
    [SerializeField] float blendSpeed;
    [SerializeField] public float rotationSpeed = 10;

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

    private RaycastHit slopeHit;
    private bool triggerOffTheGround = false;
    float _targetRotation = 0;

    enum State
    {
        idle,
        walk,
        run
    }
    [SerializeField] State state = State.idle;

    public override void UpdateAction()
    {
        if (characterBehaviour_Player.isDead) return;
        IsGrounded();
        if (isGrounded)
        {
            Movement();
            Rotation();
        }
        StickToTheGround();
    }
    private void Movement()
    {
        moveDirectionWorldRelative = new Vector3(Player_Input.Instance.movementInput.x, 0, Player_Input.Instance.movementInput.y);

        moveDirectionCameraRelative = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized * moveDirectionWorldRelative.z) + (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized * moveDirectionWorldRelative.x);
        moveDirectionCameraRelative.Normalize();

        float targetSpeed = Player_Input.Instance.isRunning ? runningSpeed : walkingSpeed;

        if (Player_Input.Instance.isRunning && Player_Input.Instance.movementInput.magnitude > 0.1f)
        {
            state = State.run;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * blendSpeed);
            characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
        }
        else if (Player_Input.Instance.movementInput.magnitude > 0.1f)
        {
            state = State.walk;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * blendSpeed);
            characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
        }
        else
        {
            state = State.idle;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime * blendSpeed);
            characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
        }

        Vector3 moveVelocity = OnSlope() * targetSpeed;

        if (canMove)
        {
            characterBehaviour_Player.rb.velocity = moveVelocity;
        }
    }
    private void RootMovement()
    {
        if (canMove)
        {
            moveDirectionWorldRelative = new Vector3(Player_Input.Instance.movementInput.x, 0, Player_Input.Instance.movementInput.y);

            moveDirectionCameraRelative = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized * moveDirectionWorldRelative.z) + (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized * moveDirectionWorldRelative.x);
            moveDirectionCameraRelative.Normalize();

            float targetSpeed = Player_Input.Instance.isRunning ? runningSpeed : walkingSpeed;

            if (Player_Input.Instance.isRunning && Player_Input.Instance.movementInput.magnitude > 0.1f)
            {
                state = State.run;
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * blendSpeed);
                characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
            }
            else if (Player_Input.Instance.movementInput.magnitude > 0.1f)
            {
                state = State.walk;
                currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * blendSpeed);
                characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
            }
            else
            {
                state = State.idle;
                currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime * blendSpeed);
                characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
            }

            Vector3 moveVelocity = OnSlope() * characterBehaviour_Player.animator.velocity.magnitude;

            characterBehaviour_Player.rb.velocity = moveVelocity;
        }
    }
    private void Rotation()
    {
        if (canRotate)
        {
            if (Player_Input.Instance.movementInput.magnitude > .1f)
            {
                _targetRotation = Mathf.Atan2(moveDirectionWorldRelative.x, moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            }
            float rotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeed);

            characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
        }
    }
    private void StickToTheGround()
    {
        if (isGrounded)
        {
            triggerOffTheGround = false;
            characterBehaviour_Player.animator.SetBool("isGrounded", true);
            if (moveDirectionWorldRelative.magnitude <= .1f)
            {
                characterBehaviour_Player.rb.velocity = Vector3.zero;
            }
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistanceDown, ~layerMask) && !triggerOffTheGround)
            {
                characterBehaviour_Player.animator.SetBool("isGrounded", true);
                rayHit = hit.point;
                characterBehaviour_Player.rb.velocity = new Vector3(characterBehaviour_Player.rb.velocity.x, -hit.point.y * 5, characterBehaviour_Player.rb.velocity.z);
            }
            else
            {
                characterBehaviour_Player.animator.SetBool("isGrounded", false);
            }
            triggerOffTheGround = true;
        }
    }
    private Vector3 OnSlope()
    {
        // Slope Movement
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, rayDistanceDown))
        {
            return Vector3.ProjectOnPlane(moveDirectionCameraRelative, slopeHit.normal).normalized;
        }
        // Horizontal Movement
        else
        {
            Vector3 vector3 = moveDirectionCameraRelative;
            vector3.y = characterBehaviour_Player.rb.velocity.y;
            return moveDirectionCameraRelative;
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
    private void OnDrawGizmos()
    {
        if (isGroundedGizmo)
        {
            // Detect Ground
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + Vector3.up * sphereOverlapHeight, sphereOverlapRadius);

            // Detect Slope
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistanceDown);
            Gizmos.DrawWireSphere(slopeHit.point, sphereOverlapRadius);

            if (characterBehaviour_Player != null)
            {
                // Forward Direction
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, OnSlope() + transform.position);
                Gizmos.DrawWireSphere(OnSlope() + transform.position, sphereOverlapRadius);
            }
        }
    }
}