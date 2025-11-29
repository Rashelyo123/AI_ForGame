using UnityEngine;
using TMPro;

public class PlayerCoin : MonoBehaviour
{
    public int totalCoins = 0;
    public TMP_Text coinText;

    void Start()
    {
        coinText.text = "Coins: " + totalCoins;
    }
    public void AddCoin(int amount)
    {
        totalCoins += amount;
        coinText.text = "Coins: " + totalCoins;
        Debug.Log("Coin Collected! Total: " + totalCoins);
    }
}
