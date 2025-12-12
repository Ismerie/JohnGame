using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GhostAI : MonoBehaviour
{
    [Header("Patrol Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Patrol")]
    public float waitTime = 0.5f;          // pause aux points
    public float snapRadius = 1.5f;        // snap des points sur le NavMesh

    [Header("Chase")]
    public float chaseDuration = 2.5f;
    public float catchDistance = 1.2f;

    [Header("Animation")]
    public Animator animator;              // si null, auto
    public string speedParam = "Speed";    // float

    private NavMeshAgent agent;
    private Transform player;

    private Vector3 homePos;
    private Quaternion homeRot;

    private bool goingToA = false;         // false = va d'abord vers B
    private bool isChasing = false;
    private bool isReturning = false;
    private bool waiting = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        // home = pointA si dispo
        homePos = (pointA != null) ? pointA.position : transform.position;
        homeRot = transform.rotation;

        if (GhostManager.Instance != null)
            GhostManager.Instance.Register(this);

        SetPatrolDestination();
    }

    void OnDestroy()
    {
        if (GhostManager.Instance != null)
            GhostManager.Instance.Unregister(this);
    }

    void Update()
    {
        if (!agent.isOnNavMesh) return;

        // anim speed
        var v = agent.velocity; v.y = 0f;
        if (animator != null)
            animator.SetFloat(speedParam, v.magnitude);

        // chase
        if (isChasing && player != null)
        {
            agent.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) <= catchDistance)
                PlayerCaught();

            return;
        }

        // return home
        if (isReturning)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.05f)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
                {
                    isReturning = false;
                    SetPatrolDestination();
                }
            }
            return;
        }

        // patrol with pause (robuste)
        if (waiting) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.05f)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
                StartCoroutine(WaitAndSwitchPatrol());
        }
    }

    IEnumerator WaitAndSwitchPatrol()
    {
        waiting = true;
        agent.isStopped = true;

        yield return new WaitForSeconds(waitTime);

        agent.isStopped = false;
        goingToA = !goingToA;
        SetPatrolDestination();

        waiting = false;
    }

    void SetPatrolDestination()
    {
        if (pointA == null || pointB == null) 
            return;

        Vector3 target = goingToA ? pointA.position : pointB.position;

        // Snap target sur le NavMesh pour éviter les points au bord / hors bleu
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, snapRadius, NavMesh.AllAreas))
            target = hit.position;

        agent.SetDestination(target);
    }

    public void Alert(Transform p)
    {
        Debug.Log("Alert");
        player = p;
        if (!isChasing)
            StartCoroutine(ChaseThenReturn());
    }

    public void DetectPlayer(Transform p)
    {
        player = p;
        if (!isChasing)
            StartCoroutine(ChaseThenReturn());
    }

    IEnumerator ChaseThenReturn()
    {
        isChasing = true;
        isReturning = false;
        waiting = false;
        Debug.Log("Chaaase");
        float t = 0f;
        while (t < chaseDuration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        isChasing = false;
        isReturning = true;

        Vector3 target = homePos;
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, snapRadius, NavMesh.AllAreas))
            target = hit.position;

        agent.SetDestination(target);
        transform.rotation = homeRot;
    }

    void PlayerCaught()
    {
        if (UIFader.Instance != null) 
            UIFader.Instance.PlayCaughtFadeAndRestart();
        else 
            StageRestarter.RestartStage();

    }
}
