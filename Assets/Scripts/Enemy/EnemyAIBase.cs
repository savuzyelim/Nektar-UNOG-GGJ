using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnemyAIBase : MonoBehaviour
{
    protected EnemyState currentState;
    protected Rigidbody2D rb;
    protected Transform player;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float stopDistance = 1f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void OnEnable()
    {
        PlayerEvents.OnCatFormEntered += OnCatForm;
        PlayerEvents.OnCombatFormEntered += OnCombatForm;
        PlayerEvents.OnMeow += OnMeowHeard;
    }

    protected virtual void OnDisable()
    {
        PlayerEvents.OnCatFormEntered -= OnCatForm;
        PlayerEvents.OnCombatFormEntered -= OnCombatForm;
        PlayerEvents.OnMeow -= OnMeowHeard;
    }

    protected virtual void FixedUpdate()
    {
        StateUpdate();
    }

    protected void MoveTowards(Vector2 target)
    {
        Vector2 dir = target - rb.position;

        if (dir.magnitude <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = dir.normalized * moveSpeed;
    }

    protected abstract void StateUpdate();

    // Event hook�lar� (override edilecek)
    protected virtual void OnMeowHeard(Vector2 meowPos) { }
    protected virtual void OnCatForm() { }
    protected virtual void OnCombatForm() { }
}
