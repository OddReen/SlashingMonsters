using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : CharacterBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;

    Rigidbody rb;
    [SerializeField] float speed = 10f;
    [SerializeField] float distanceToSight = 10f;
    [SerializeField] float elapsedTime = 0;
    [SerializeField] LayerMask sightLayer;
    [SerializeField] bool isSighted = false;
    [SerializeField] bool canWalk = true;
    [SerializeField] bool isRootAnimating = false;

    [SerializeField] Vector3[] pathPoints;

    enum State
    {
        Idle,
        Patrol,
        Chase,
        Combat
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        StartCoroutine(IsSighted());
        StartCoroutine(OnAnimation());
    }
    private void LateUpdate()
    {
        animator.transform.position = transform.position;
    }
    private void FixedUpdate()
    {
        if (canWalk)
        {
            //GoAroundPlayer();
        }
    }
    IEnumerator OnAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
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
    }
    void Patrol()
    {

    }
    void Chase()
    {

    }
    void GoAroundPlayer()
    {
        if (GameManager.Instance.player != null)
        {
            Vector3 direction = GameManager.Instance.player.transform.position - transform.position;

            Quaternion rotation = Quaternion.LookRotation(direction);

            //Quaternion.Lerp(transform.rotation, rotation, 100 * Time.deltaTime);

            rb.rotation = rotation;

            rb.velocity = transform.right * speed * Time.deltaTime;
        }
    }
    IEnumerator IsSighted()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            if (GameManager.Instance.player != null)
            {
                Vector3 dir = (GameManager.Instance.player.transform.position + Vector3.up) - (transform.position + Vector3.up);
                dir.Normalize();
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, dir, out hit, distanceToSight, sightLayer))
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
    private void OnDrawGizmos()
    {
        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            if (isSighted)
                Gizmos.color = Color.green;
            else
                Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position + Vector3.up, GameManager.Instance.player.transform.position + Vector3.up);

            NavMeshPath path = new NavMeshPath();
            bool pathFound = NavMesh.CalculatePath(transform.position, GameManager.Instance.player.transform.position, navMeshAgent.areaMask, path);
            if (pathFound)
            {
                pathPoints = path.corners;
                for (int i = 0; i < pathPoints.Length; i++)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(pathPoints[i], .5f);
                }
            }
            else
            {
                Debug.LogWarning("Failed to find a valid path to the player.");
            }
        }
    }
}