using UnityEngine;
using System.Collections;

public class NPCController : MonoBehaviour
{
    public enum NPCState { Idle, Wandering, Alerted, Following, Hurt, Panicking }
    public NPCState currentState = NPCState.Idle;

    [Header("Ayarlar")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float wanderRadius = 5f;

    private Transform player;
    private Vector2 targetPosition;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // Oyuncuyu bulma işlemini sağlama alalım
        FindPlayer();

        targetPosition = transform.position;
        StartCoroutine(WanderRoutine());
    }

    void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void Update()
    {
        // Eğer player bir şekilde kaybolduysa tekrar ara (Örn: Sahnede yeniden doğma)
        if (player == null) FindPlayer();

        HandleStates();

        if (Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            if (rb.linearVelocity.x > 0 && !isFacingRight) Flip();
            else if (rb.linearVelocity.x < -0.1f && isFacingRight) Flip();
        }
    }

    void HandleStates()
    {
        switch (currentState)
        {
            case NPCState.Idle:
                rb.linearVelocity = Vector2.zero;
                UpdateAnimations("isIdle");
                break;

            case NPCState.Wandering:
                MoveTowards(targetPosition, walkSpeed);
                UpdateAnimations("isWalking");
                if (Vector2.Distance(transform.position, targetPosition) < 0.3f) currentState = NPCState.Idle;
                break;

            case NPCState.Alerted:
                rb.linearVelocity = Vector2.zero;
                UpdateAnimations("isAlert");
                break;

            case NPCState.Following:
                if (player != null)
                {
                    // Oyuncuya çok yaklaşınca durması için mesafe kontrolü (Opsiyonel)
                    if (Vector2.Distance(transform.position, player.position) > 1.2f)
                    {
                        MoveTowards(player.position, walkSpeed);
                        UpdateAnimations("isWalking");
                    }
                    else
                    {
                        rb.linearVelocity = Vector2.zero;
                        UpdateAnimations("isIdle");
                    }
                }
                break;

            case NPCState.Hurt:
                rb.linearVelocity = Vector2.zero;
                UpdateAnimations("isHurt");
                break;

            case NPCState.Panicking:
                if (player != null)
                {
                    Vector2 runDirection = (transform.position - player.position).normalized;
                    rb.linearVelocity = runDirection * runSpeed;
                    UpdateAnimations("isRunning");
                }
                break;
        }
    }

    public void GetCalled()
    {
        if (currentState != NPCState.Panicking && currentState != NPCState.Hurt)
        {
            StopAllCoroutines();
            StartCoroutine(AlertRoutine());
        }
    }

    IEnumerator AlertRoutine()
    {
        currentState = NPCState.Alerted;
        yield return new WaitForSeconds(1f); // 1 saniye şaşkınlık
        currentState = NPCState.Following;
    }

    void MoveTowards(Vector2 target, float speed)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * speed;
    }

    void UpdateAnimations(string activeParam)
    {
        if (anim == null) return;
        if (!anim.GetBool(activeParam))
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            anim.SetBool("isAlert", false);
            anim.SetBool("isHurt", false);
            anim.SetBool("isIdle", false);
            anim.SetBool(activeParam, true);
        }
    }

    // Diğer yardımcı fonksiyonlar (Wander, Flip, GetHit vb.) aynı kalsın...
    IEnumerator WanderRoutine()
    {
        while (true)
        {
            if (currentState == NPCState.Idle)
            {
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                targetPosition = (Vector2)transform.position + Random.insideUnitCircle * wanderRadius;
                currentState = NPCState.Wandering;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
}