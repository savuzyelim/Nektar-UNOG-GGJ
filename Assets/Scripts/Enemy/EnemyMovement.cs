using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float stopDistance = 1f;

    Rigidbody2D rb;
    Transform player;

    public EnemyState CurrentState { get; private set; } = EnemyState.Idle;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {

        Vector2 dir = player.position - transform.position;
        float distance = dir.magnitude;

        if(CurrentState == EnemyState.Investigating)
        {
            if (distance > stopDistance)
                rb.linearVelocity = dir.normalized * moveSpeed;
            else
                rb.linearVelocity = Vector2.zero;
        }
    }

    public void SetState(EnemyState newState)
    {
        CurrentState = newState;
    }
}
