using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


[RequireComponent(typeof(NavMeshAgent))]
public class JohnMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }

        Vector3 horizontalVelocity = agent.velocity;
        horizontalVelocity.y = 0f;
        float speed = horizontalVelocity.magnitude;
        animator.SetFloat("Speed", speed);
    }

}
