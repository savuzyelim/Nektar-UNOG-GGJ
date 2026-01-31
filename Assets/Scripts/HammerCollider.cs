using UnityEngine;

public class HammerCollider : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int hammerDamage;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<EnemyHealth>() != null)
        {
            rb.linearVelocity = Vector2.zero;
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(hammerDamage);
        }
    }
}
