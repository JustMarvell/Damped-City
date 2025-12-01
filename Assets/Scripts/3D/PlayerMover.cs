using UnityEngine;
using UnityEngine.AI;

public class PlayerMover : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;

    public LayerMask groundLayer;

    private InputManager_3D input;

    private bool mouseClick => input.clickInput;
    private Vector2 mousePosition => input.mousePositionInput;
    
    private Camera cam;
    private Animator animator;

    void Start()
    {
        cam = Camera.main;
        input = GetComponent<InputManager_3D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (mouseClick)
        {
            Ray ray = cam.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 999999f, groundLayer))
            {
                agent.speed = speed;
                agent.SetDestination(hit.point);
            }
        }

        if (animator != null)
        {
            animator.SetFloat("speed", agent.velocity.sqrMagnitude);
        }
    }
}
