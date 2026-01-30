using UnityEngine;

public class CatController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private bool isFacingRight = true;

    [Header("Mod Ayarları")]
    public bool isMasked = false;
    public GameObject puffVFXPrefab;

    [Header("Miyavlama Ayarları")]
    public float meowRange = 5f;     // Miyavlamanın etki alanı
    public KeyCode meowKey = KeyCode.F; // Miyavlama tuşu (F)

    [Header("Bileşenler")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 1. Hareket Girdisi
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        // 2. Mod Değiştirme (E tuşu)
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchMode();
        }

        // 3. Miyavlama (Sadece maskesizken/4 ayak üzerindeyken)
        if (!isMasked && Input.GetKeyDown(meowKey))
        {
            Meow();
        }

        // 4. Yön Çevirme (Flip)
        if (moveInput.x > 0 && !isFacingRight) Flip();
        else if (moveInput.x < 0 && isFacingRight) Flip();

        // 5. Animasyon Parametrelerini Güncelle
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Unity 2023+ sürümleri için linearVelocity, eskiler için velocity kullanır
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    void Meow()
    {
        // Eğer miyavlama animasyonun varsa buraya anim.SetTrigger("isMeowing") ekleyebilirsin
        Debug.Log("Miyavlandı, NPC'ler çağrılıyor...");

        // Çevredeki NPC'leri bul
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, meowRange);

        foreach (Collider2D hit in hits)
        {
            // NPC'nin üzerindeki scripti bul ve çağır
            NPCController npc = hit.GetComponent<NPCController>();
            if (npc != null)
            {
                npc.GetCalled(); // Önceki yazdığımız 1 sn alert + takip fonksiyonu
            }
        }
    }

    void SwitchMode()
    {
        isMasked = !isMasked;

        if (puffVFXPrefab != null)
        {
            Instantiate(puffVFXPrefab, transform.position, Quaternion.identity);
        }

        anim.SetBool("isMasked", isMasked);
    }

    void UpdateAnimations()
    {
        bool isMoving = moveInput.magnitude > 0;
        anim.SetBool("isMoving", isMoving);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    // Editörde miyavlama alanını görebilmek için
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, meowRange);
    }
}