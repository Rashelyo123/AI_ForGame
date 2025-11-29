using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerCoin playerCoin = collision.GetComponent<PlayerCoin>();
            if (playerCoin != null)
            {
                playerCoin.AddCoin(coinValue);
            }

            Destroy(gameObject);
        }
    }
}
