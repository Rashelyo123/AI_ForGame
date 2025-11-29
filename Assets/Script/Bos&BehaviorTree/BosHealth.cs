using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxHP = 100;
    public float currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        GetComponent<BossAI>().health = currentHP;
    }
}
