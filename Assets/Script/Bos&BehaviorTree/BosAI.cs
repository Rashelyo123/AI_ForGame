using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("Stats")]
    public float health = 100f;
    public float attackRange = 5f;
    public float safeDistance = 4f;
    public float moveSpeed = 2f;

    [Header("References")]
    public Transform player;
    public GameObject normalBulletPrefab;
    public GameObject specialBulletPrefab;

    public void MoveTowardsPlayerSafe()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > safeDistance)
        {
            // Mendekat
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Jangan terlalu dekat, stop
        }
    }

    public bool PlayerInAttackRange()
    {
        return Vector2.Distance(transform.position, player.position) <= attackRange;
    }

    public void NormalAttack()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        GameObject bullet = Instantiate(normalBulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * 7f;
    }

    public void SpecialAttack()
    {
        int bulletCount = 20;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            GameObject bullet = Instantiate(specialBulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = dir * 6f;
        }
    }


    public bool CanSeePlayer()
    {
        return Vector2.Distance(transform.position, player.position) < 15f;
    }
}
