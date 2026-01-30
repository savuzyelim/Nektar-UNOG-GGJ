using UnityEngine;

public class CatController : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private bool isFacingRight = true;

    [Header("Mod Ayarlar�")]
    public bool isMasked = false;
    public GameObject puffVFXPrefab; // Puff efekti i�in 2D obje/prefab

    [Header("Bile�enler")]
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

        // 2. Mod De�i�tirme (�rn: E tu�u veya Space)
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchMode();
        }

        // 3. Y�n �evirme (Flip)
        if (moveInput.x > 0 && !isFacingRight) Flip();
        else if (moveInput.x < 0 && isFacingRight) Flip();

        // 4. Animasyon Parametrelerini G�ncelle
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    void SwitchMode()
    {
        isMasked = !isMasked;

        // VFX Olu�turma
        if (puffVFXPrefab != null)
        {
            Instantiate(puffVFXPrefab, transform.position, Quaternion.identity);
        }

        // Animator'e hangi modda oldu�umuzu bildiriyoruz
        anim.SetBool("isMasked", isMasked);
    }

    void UpdateAnimations()
    {
        // Hareket ediyor mu kontrol�
        bool isMoving = moveInput.magnitude > 0;
        anim.SetBool("isMoving", isMoving);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        // Pixel art oyunlarda scale �evirmek, pivot kaymas�n� �nlemek i�in en temizidir
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}