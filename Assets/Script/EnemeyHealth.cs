using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float health = 100f;

    [Header("Damage Effects")]
    public float flashDuration = 0.1f;
    public float knockbackForce = 3f;
    public float hitScalePunch = 0.1f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Vector3 originalScale;
    private Rigidbody2D rb;

    public CoinSpawner coinSpawner;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        originalColor = sr.color;
        originalScale = transform.localScale;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        // EFFECTS
        StartCoroutine(DamageFlash());
        StartCoroutine(HitScalePunchEffect());

        if (rb != null)
            KnockbackEffect();

        if (health <= 0)
        {
            Die();
        }
    }


    System.Collections.IEnumerator DamageFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }


    void KnockbackEffect()
    {
        Vector2 knockDir = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
        rb.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);
    }


    System.Collections.IEnumerator HitScalePunchEffect()
    {
        Vector3 punchScale = originalScale * (1f + hitScalePunch);
        transform.localScale = punchScale;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = originalScale;
    }


    void Die()
    {
        if (coinSpawner != null)
            coinSpawner.SpawnCoin();

        Destroy(gameObject);
    }
}
