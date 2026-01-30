using UnityEngine;
using System.Collections;

public class EnemyAbilityResponse : MonoBehaviour
{
    Rigidbody2D rb;
    EnemyMovement enemyMovement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    void OnEnable()
    {
        AbilityEvents.OnAbilityUsed += OnAbilityUsed;
    }

    void OnDisable()
    {
        AbilityEvents.OnAbilityUsed -= OnAbilityUsed;
    }

    

    void OnAbilityUsed(AbilityType type,Vector2 center,float radius,float force)
    {
        Vector2 diff = (Vector2)transform.position - center;

        if (diff.sqrMagnitude > radius * radius)
            return;

        switch (type)
        {
            case AbilityType.Pull:
                enemyMovement.SetState(EnemyState.Pulled);
                break;

            case AbilityType.Knockback:
                StartCoroutine(WaitTheKnockback(diff, force));
                break;
        }
    }
    IEnumerator WaitTheKnockback(Vector2 diff, float force)
    {
        enemyMovement.SetState(EnemyState.Idle);
        rb.AddForce(diff.normalized * force, ForceMode2D.Impulse);
        yield return new WaitForSeconds(.5f);
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(.5f);
        enemyMovement.SetState(EnemyState.Pulled);
    }
}
