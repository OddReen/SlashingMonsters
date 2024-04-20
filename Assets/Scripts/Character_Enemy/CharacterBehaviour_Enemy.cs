using System.Collections;
using UnityEngine;
using UnityEngine.AI;
public class CharacterBehaviour_Enemy : CharacterBehaviour
{
    [SerializeField] bool onGizmos = false;
    [SerializeField] float attackDelay = 1f;
    [SerializeField] float rangeToCombat = 1f;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float speed = 10f;
    [SerializeField] float distanceToSight = 10f;
    [SerializeField] float elapsedTime = 0;
    [SerializeField] LayerMask sightLayer;
    [SerializeField] bool inRange = false;
    [SerializeField] bool isSighted = false;
    [SerializeField] bool canWalk = true;
    [SerializeField] bool isRootAnimating = false;
    [SerializeField] bool pathFound = false;

    [SerializeField] Vector3[] pathPoints;

    NavMeshPath path;

    Coroutine c_InRange;
    Coroutine c_IsSighted;
    Coroutine c_Attack;

    [SerializeField] State state;
    enum State
    {
        Idle,
        Chase,
        Combat
    }
    private void Start()
    {
        path = new NavMeshPath();

        c_IsSighted = StartCoroutine(IsSighted());
        c_InRange = StartCoroutine(InRange());
    }
    private void LateUpdate()
    {
        animator.transform.position = transform.position;
    }
    private void FixedUpdate()
    {
        if (isDead) return;
        float move = 0f;
        if (isSighted && inRange)
        {
            state = State.Combat;
            move = 0;
            RotateTowards(GameManager.Instance.player.transform.position);
            if (c_Attack == null)
                c_Attack = StartCoroutine(Attack());
        }
        else if (isSighted && !inRange)
        {
            state = State.Chase;
            move = 3;
            MoveToPlayer();
            if (pathPoints.Length != 0 && pathPoints[1] != null)
            {
                RotateTowards(pathPoints[1]);
            }
        }
        else
        {
            state = State.Idle;
        }
        animator.SetFloat("Move", move);
    }
    private void MoveToPlayer()
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
    IEnumerator Attack()
    {
        while (inRange)
        {
            yield return new WaitForSeconds(attackDelay);
            canWalk = false;

            animator.SetBool("hasAttacked", true);
            yield return new WaitForEndOfFrame();
            animator.SetBool("hasAttacked", false);

            rb.velocity = Vector3.zero;
            isRootAnimating = true;
            elapsedTime = 0;
            while (elapsedTime <= animator.GetCurrentAnimatorStateInfo(0).length * animator.GetCurrentAnimatorStateInfo(0).speedMultiplier && animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
            {
                rb.velocity = new Vector3(animator.velocity.x, rb.velocity.y, animator.velocity.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            isRootAnimating = false;
            rb.velocity = Vector3.zero;
            canWalk = true;
        }
        c_Attack = null;
    }
    IEnumerator IsSighted()
    {
        while (!inRange)
        {
            yield return new WaitForSeconds(0.25f);
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
    IEnumerator InRange()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
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
