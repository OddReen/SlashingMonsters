using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class CharacterBehaviour_Enemy : CharacterBehaviour
{
    [SerializeField] State state;
    enum State
    {
        Idle,
        Chase,
        Attack
    }

    NavMeshPath path;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] Vector3[] pathPoints;

    [Header("Combat")]
    [SerializeField] float attackRotateSpeed = 10f;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float rangeToCombat = 1f;

    [Header("Observations")]
    [SerializeField] float distanceToSight = 10f;
    [SerializeField] bool inRange = false;
    [SerializeField] public bool isSighted = false;
    [SerializeField] LayerMask sightLayer;

    [Header("Booleans")]
    [SerializeField] bool isPerformingAction = false;
    [SerializeField] bool pathFound = false;
    [SerializeField] bool onGizmos = false;

    private void Start()
    {
        state = State.Idle;

        path = new NavMeshPath();

        StartCoroutine(C_StateUpdate());
        StartCoroutine(C_IsSighted());
        StartCoroutine(C_InRange());
    }
    private void LateUpdate()
    {
        animator.transform.position = transform.position;
    }
    private void FixedUpdate()
    {
        if (isDead || isStunned || GameManager.Instance.player == null) return;
        switch (state)
        {
            case State.Chase:
                animator.SetBool("isMoving", true);
                Chase_Movement();
                Chase_Rotation();
                break;
            case State.Attack:
                animator.SetBool("isMoving", false);
                StartCoroutine(C_Attack());
                break;
            default:
                break;
        }
    }

    private void Chase_Movement()
    {
        pathFound = NavMesh.CalculatePath(transform.position, GameManager.Instance.player.transform.position, -1, path);
        if (pathFound)
        {
            pathPoints = path.corners;
            Vector3 direction = pathPoints[1] - transform.position;
            direction.Normalize();
            rb.velocity = direction * moveSpeed;
        }
    }
    void Chase_Rotation()
    {
        if (pathPoints.Length != 0 && pathPoints[1] != null)
        {
            Vector3 direction = pathPoints[1] - transform.position;

            float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float smoothRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, rotateSpeed * Time.deltaTime);

            rb.MoveRotation(Quaternion.Euler(0.0f, smoothRotation, 0.0f));
        }
    }
    void Attack_Rotation()
    {
        Vector3 direction = GameManager.Instance.player.transform.position - transform.position;

        float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        float smoothRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, attackRotateSpeed * Time.deltaTime);

        rb.MoveRotation(Quaternion.Euler(0.0f, smoothRotation, 0.0f));
    }

    IEnumerator C_StateUpdate()
    {
        while (true)
        {
            if (isSighted && inRange)
            {
                state = State.Attack;
            }
            else if (isSighted && !inRange && !isPerformingAction)
            {
                state = State.Chase;
            }
            else
            {
                state = State.Idle;
            }
            yield return null;
        }
    }
    IEnumerator C_Attack()
    {
        isPerformingAction = true;
        yield return new WaitForSeconds(attackDelay);

        animator.SetBool("hasAttacked", true);
        yield return new WaitForEndOfFrame();
        animator.SetBool("hasAttacked", false);

        rb.velocity = Vector3.zero;
        while (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") || animator.GetNextAnimatorStateInfo(0).IsTag("Attack"))
        {
            rb.velocity = new Vector3(animator.velocity.x, rb.velocity.y, animator.velocity.z);
            yield return null;
        }
        rb.velocity = Vector3.zero;
        isPerformingAction = false;
    }
    IEnumerator C_IsSighted()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (GameManager.Instance.player != null)
            {
                Vector3 dir = (GameManager.Instance.player.transform.position + Vector3.up) - (transform.position + Vector3.up);
                dir.Normalize();
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, dir, out hit, distanceToSight, ~sightLayer))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        isSighted = true;
                    }
                    else
                    {
                        isSighted = false;
                    }
                }
                else
                {
                    isSighted = false;
                }

            }
        }
    }
    IEnumerator C_InRange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (isSighted)
            {
                if (GameManager.Instance.player != null)
                {
                    if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) <= rangeToCombat)
                    {
                        inRange = true;
                    }
                    else
                    {
                        inRange = false;
                    }
                }
                else
                {
                    inRange = false;
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (onGizmos)
        {
            // Path
            if (pathFound)
            {
                for (int i = 0; i < pathPoints.Length; i++)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(pathPoints[i], .5f);
                }
            }
            // Sight Player
            if (GameManager.Instance != null && GameManager.Instance.player != null)
            {
                Gizmos.color = isSighted ? Color.green : Color.red;
                Gizmos.DrawLine(transform.position + Vector3.up, GameManager.Instance.player.transform.position + Vector3.up);
            }
        }
    }
}
