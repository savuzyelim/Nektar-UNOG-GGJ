using UnityEngine;

public class CatTransform : MonoBehaviour
{
    public bool normalCat;
    [SerializeField] GameObject hammer;
    [SerializeField] SpriteRenderer sprite;
    [Header("Pull")]
    [SerializeField] float pullRadius = 6f;
    [SerializeField] float pullForce = 5f;

    [Header("Knockback")]
    [SerializeField] float knockbackRadius = 5f;
    [SerializeField] float knockbackForce = 15f;
    [SerializeField] int knockbackDammage = 1;
    private void Start()
    {
        normalCat = true;
        ChangeCatStands(normalCat);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            normalCat = !normalCat;
            ChangeCatStands(normalCat);
        }
        if (Input.GetKeyDown(KeyCode.E) && normalCat)
        {
            AbilityEvents.OnAbilityUsed?.Invoke(AbilityType.Pull, transform.position, pullRadius, pullForce, 0);
        }
    }

    private void ChangeCatStands(bool catStand)
    {
        if (normalCat)
        {
            sprite.color = Color.white;
            hammer.SetActive(false);
        }
        else
        {
            sprite.color = Color.green;
            hammer.SetActive(true);
            AbilityEvents.OnAbilityUsed?.Invoke(AbilityType.Knockback, transform.position, knockbackRadius, knockbackForce, knockbackDammage);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, knockbackRadius);
    }
}
