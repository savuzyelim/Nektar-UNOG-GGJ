using UnityEngine;
using UnityEngine.SceneManagement;

public class Hammer : MonoBehaviour
{
    public Camera cam;
    Vector2 mousePos;
    [SerializeField] float angleOffset;
    [SerializeField] Rigidbody2D hammerRb;
    [SerializeField] GameObject hammerObj;
    bool canThrow;
    [SerializeField] float throwForce;
    [SerializeField] float hammerMoveSpeed;

    private void Start()
    {
        canThrow = true;
    }
    private void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //HAmmerý fýrlat
        if (canThrow && Input.GetMouseButtonDown(0))
        {
            hammerObj.transform.parent = null;
            hammerRb.AddForce(hammerObj.transform.up * throwForce, ForceMode2D.Impulse);
            canThrow = false;
        }
        //Hammerý geri çaðýr
        if(!canThrow)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (Vector2.Distance(transform.position, hammerObj.transform.position) > 1f)
                {
                    Vector2 dir = transform.position - hammerObj.transform.position;
                    hammerRb.linearVelocity = dir.normalized * hammerMoveSpeed;
                }
            }
            if(Vector2.Distance(transform.position, hammerObj.transform.position) <= 1f)
            {
                hammerRb.linearVelocity = Vector2.zero;
                hammerObj.transform.SetParent(this.transform);
                canThrow = true;
                Debug.Log(canThrow);
            }
        }
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene("EnesScene");
        }
    }
    private void FixedUpdate()
    {
        Vector2 hammerDir = mousePos - new Vector2(transform.position.x, transform.position.z);
        float angle = Mathf.Atan2(hammerDir.y, hammerDir.x) * Mathf.Rad2Deg - angleOffset;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
