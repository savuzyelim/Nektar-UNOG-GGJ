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
    [SerializeField] int knockbackDamage = 1;

    private void Start()
    {
        normalCat = true;
        EnterCatForm();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            normalCat = !normalCat;

            if (normalCat)
                EnterCatForm();
            else
                EnterCombatForm();
        }

        // Miyav (Pull)
        if (Input.GetKeyDown(KeyCode.E) && normalCat)
        {
            // Ability sistemi
            AbilityEvents.OnAbilityUsed?.Invoke(
                AbilityType.Pull,
                transform.position,
                pullRadius,
                pullForce,
                0
            );

            // Enemy AI'lar için event
            PlayerEvents.OnMeow?.Invoke(transform.position);
        }
    }

    private void EnterCatForm()
    {
        sprite.color = Color.white;
        hammer.SetActive(false);

        PlayerEvents.OnCatFormEntered?.Invoke();
    }

    private void EnterCombatForm()
    {
        sprite.color = Color.green;
        hammer.SetActive(true);

        // Knockback ability
        AbilityEvents.OnAbilityUsed?.Invoke(
            AbilityType.Knockback,
            transform.position,
            knockbackRadius,
            knockbackForce,
            knockbackDamage
        );

        PlayerEvents.OnCombatFormEntered?.Invoke();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, pullRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, knockbackRadius);
    }
}
