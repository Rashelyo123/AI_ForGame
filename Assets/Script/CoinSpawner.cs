using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Prefabs")]
    public GameObject bronzeCoin;
    public GameObject silverCoin;
    public GameObject goldCoin;

    [Header("Probabilitas (%)")]
    public float bronzeChance = 50f;
    public float silverChance = 30f;
    public float goldChance = 20f;

    public void SpawnCoin()
    {
        float roll = Random.Range(0f, 100f);

        Vector3 pos = transform.position;

        if (roll < bronzeChance)
        {
            Instantiate(bronzeCoin, pos, Quaternion.identity);
        }
        else if (roll < bronzeChance + silverChance)
        {
            Instantiate(silverCoin, pos, Quaternion.identity);
        }
        else
        {
            Instantiate(goldCoin, pos, Quaternion.identity);
        }
    }
}
