using UnityEngine;
using UnityEngine.SceneManagement;

public class Hammer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Camera cam;
    [SerializeField] Rigidbody2D hammerRb;
    [SerializeField] Transform hammer;

    [Header("Settings")]
    [SerializeField] float throwForce = 10f;
    [SerializeField] float returnSpeed = 12f;
    [SerializeField] float angleOffset = 90f;
    [SerializeField] float attachDistance = 0.8f;

    Vector2 mousePos;
    HammerState currentState = HammerState.Attached;

    void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        HandleInput();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene("EnesScene");
    }

    void FixedUpdate()
    {
        RotateHammer();

        switch (currentState)
        {
            case HammerState.Returning:
                ReturnMovement();
                break;
        }
    }

    // ================= INPUT =================

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && currentState == HammerState.Attached)
            Throw();

        if (Input.GetMouseButtonDown(1) && currentState == HammerState.Thrown)
            StartReturn();
    }

    // ================= STATES =================

    void Throw()
    {
        currentState = HammerState.Thrown;

        hammer.SetParent(null);
        hammerRb.linearVelocity = Vector2.zero;
        hammerRb.angularVelocity = 0f;

        hammerRb.AddForce(hammer.up * throwForce, ForceMode2D.Impulse);
    }

    void StartReturn()
    {
        currentState = HammerState.Returning;
    }

    void ReturnMovement()
    {
        Vector2 dir = (Vector2)transform.position - hammerRb.position;
        hammerRb.linearVelocity = dir.normalized * returnSpeed;

        if (dir.magnitude <= attachDistance)
            Attach();
    }

    void Attach()
    {
        currentState = HammerState.Attached;

        hammerRb.linearVelocity = Vector2.zero;
        hammerRb.angularVelocity = 0f;

        hammer.SetParent(transform);
        hammer.localPosition = new Vector3(0,1.2f,0);
        hammer.localRotation = Quaternion.identity;
    }

    // ================= ROTATION =================

    void RotateHammer()
    {
        Vector2 dir = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
