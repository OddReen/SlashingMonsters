using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class CharacterBehaviour_Enemy : CharacterBehaviour
{
    [SerializeField] bool onGizmos = false;

    NavMeshPath path;

    [Header("Movement")]
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float speed = 10f;
    [SerializeField] Vector3[] pathPoints;

    [Header("Combat")]
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float rangeToCombat = 1f;

    [Header("Observations")]
    [SerializeField] float distanceToSight = 10f;
    [SerializeField] bool playerInRange = false;
    [SerializeField] bool playerSighted = false;
    [SerializeField] LayerMask sightLayer;

    [Header("Booleans")]
    [SerializeField] bool canWalk = true;
    [SerializeField] bool pathFound = false;

    Coroutine c_InRange;
    Coroutine c_IsSighted;
    Coroutine c_Attack;

    private void Start()
    {
        path = new NavMeshPath();

        c_IsSighted = StartCoroutine(C_IsSighted());
        c_InRange = StartCoroutine(C_InRange());
    }
    private void LateUpdate()
    {
        animator.transform.position = transform.position;
    }
    private void FixedUpdate()
    {
        if (isDead || isStunned) return;
        float move = 0f;
        if (playerSighted && playerInRange)
        {
            move = 0;
            RotateTowards(GameManager.Instance.player.transform.position);
            if (c_Attack == null)
                c_Attack = StartCoroutine(C_Attack());
        }
        else if (playerSighted && !playerInRange)
        {
            move = 3;
            MoveToPlayer();
            if (pathPoints.Length != 0 && pathPoints[1] != null)
            {
                RotateTowards(pathPoints[1]);
            }
        }
        animator.SetFloat("Move", move);
    }
    private void MoveToPlayer()
    {
        if (canWalk)
        {
            pathFound = NavMesh.CalculatePath(transform.position, GameManager.Instance.player.transform.position, -1, path);
            if (pathFound)
            {
                pathPoints = path.corners;
                Vector3 direction = pathPoints[1] - transform.position;
                direction.Normalize();
                rb.velocity = direction * speed;
            }
            else
            {
                Debug.LogWarning("No Path");
            }
        }
    }
    void RotateTowards(Vector3 target)
    {
        if (GameManager.Instance.player != null)
        {
            Vector3 direction = target - transform.position;

            float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            float smoothRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, rotateSpeed * Time.deltaTime);

            rb.MoveRotation(Quaternion.Euler(0.0f, smoothRotation, 0.0f));
        }
    }
    IEnumerator C_Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        canWalk = false;

        animator.SetBool("hasAttacked", true);
        yield return new WaitForEndOfFrame();
        animator.SetBool("hasAttacked", false);

        rb.velocity = Vector3.zero;
        while (animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            rb.velocity = new Vector3(animator.velocity.x, rb.velocity.y, animator.velocity.z);
            yield return null;
        }
        rb.velocity = Vector3.zero;
        canWalk = true;
        c_Attack = null;
    }

    IEnumerator C_IsSighted()
    {
        while (!playerInRange)
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
                        playerSighted = true;
                    }
                    else
                    {
                        playerSighted = false;
                    }
                }
                else
                {
                    playerSighted = false;
                }

            }
        }
    }
    IEnumerator C_InRange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (playerSighted)
            {
                if (GameManager.Instance.player != null)
                {
                    if (Vector3.Distance(transform.position, GameManager.Instance.player.transform.position) <= rangeToCombat)
                    {
                        playerInRange = true;
                    }
                    else
                    {
                        playerInRange = false;
                    }
                }
                else
                {
                    playerInRange = false;
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
                Gizmos.color = playerSighted ? Color.green : Color.red;
                Gizmos.DrawLine(transform.position + Vector3.up, GameManager.Instance.player.transform.position + Vector3.up);
            }
        }
    }
}
