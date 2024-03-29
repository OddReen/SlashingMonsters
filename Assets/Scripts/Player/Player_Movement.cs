using UnityEngine;

public class Player_Movement : PlayerActions
{
    [SerializeField] public bool canMove = true;

    [Header("Stats")]
    public float currentSpeed;
    [SerializeField] float walkingSpeed = 5;
    [SerializeField] float runningSpeed = 8;
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
    bool triggerOffTheGround = false;

    [Header("Bolleans")]
    [SerializeField] public bool canRotate = true;

    public override void Action()
    {
        IsGrounded();
        if (canMove)
        {
            Movement();
            Rotation();
        }
        //StickToTheGround();
    }
    private void Movement()
    {
        if (!isGrounded)
            return;
        moveDirectionWorldRelative = new Vector3(Player_Input.Instance.movementInput.x, 0, Player_Input.Instance.movementInput.y);

        moveDirectionCameraRelative = (new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized * moveDirectionWorldRelative.z) + (new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized * moveDirectionWorldRelative.x);
        moveDirectionCameraRelative.Normalize();

        float targetSpeed = Player_Input.Instance.isRunning ? runningSpeed : walkingSpeed;

        if (Player_Input.Instance.isRunning && Player_Input.Instance.movementInput.magnitude > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * blendSpeed);
            characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
        }
        else if (Player_Input.Instance.movementInput.magnitude > 0.1f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, Time.deltaTime * blendSpeed);
            characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, Time.deltaTime * blendSpeed);
            characterBehaviour_Player.animator.SetFloat("Move", currentSpeed);
        }
        Vector3 moveVelocity = moveDirectionCameraRelative * targetSpeed;

        moveVelocity.y = characterBehaviour_Player.rb.velocity.y;

        characterBehaviour_Player.rb.velocity = moveVelocity;
    }
    private void Rotation()
    {
        if (moveDirectionWorldRelative != Vector3.zero && canRotate)
        {
            float _targetRotation = Mathf.Atan2(moveDirectionWorldRelative.x, moveDirectionWorldRelative.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float rotation = Mathf.LerpAngle(transform.eulerAngles.y, _targetRotation, Time.deltaTime * rotationSpeed);

            characterBehaviour_Player.rb.MoveRotation(Quaternion.Euler(0.0f, rotation, 0.0f));
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
    private void StickToTheGround()
    {
        if (isGrounded)
        {
            triggerOffTheGround = false;
            characterBehaviour_Player.animator.SetBool("isGrounded", true);
        }
        if (isGrounded && moveDirectionWorldRelative.magnitude <= .1f)
        {
            characterBehaviour_Player.rb.velocity = Vector3.zero;
        }
        if (!isGrounded)
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
    private void ChangeModelPos()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistanceDown, ~layerMask) && !triggerOffTheGround)
        {
            rayHit = hit.point;
            Vector3 vector3 = transform.position;
            vector3.y = rayHit.y;
            characterBehaviour_Player.animator.gameObject.transform.position = Vector3.Lerp(characterBehaviour_Player.animator.gameObject.transform.position, vector3, Time.deltaTime * 25);
        }
        else
        {
            characterBehaviour_Player.animator.gameObject.transform.position = transform.position;
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