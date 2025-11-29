using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3f;
    public int damage = 20;
    public string targetTag = "Enemy";

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            EnemyHealth hp = other.GetComponent<EnemyHealth>();
            BossHealth bhp = other.GetComponent<BossHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
            if (bhp != null)
            {
                bhp.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
