using UnityEngine;

public class EnemyMerakliFareci : EnemyAIBase
{
    Vector2 investigateTarget;
    Vector2 panicDirection;

    protected override void Awake()
    {
        base.Awake();
        currentState = EnemyState.Idle;
    }

    protected override void StateUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                rb.linearVelocity = Vector2.zero;
                break;

            case EnemyState.Investigating:
                MoveTowards(investigateTarget);
                break;

            case EnemyState.Panic:
                rb.linearVelocity = panicDirection * moveSpeed;
                break;
        }
    }

    protected override void OnMeowHeard(Vector2 meowPos)
    {
        if (currentState != EnemyState.Idle)
            return;

        investigateTarget = meowPos;
        currentState = EnemyState.Investigating;
    }

    protected override void OnCombatForm()
    {
        currentState = EnemyState.Panic;
        panicDirection = Random.insideUnitCircle.normalized;
    }

    protected override void OnCatForm()
    {
        currentState = EnemyState.Idle;
    }
}
