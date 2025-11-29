using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Objective Settings")]
    public int totalCakesRequired = 3;
    private int collectedCakes = 0;

    [Header("UI")]
    public TMP_Text cakeText;

    [Header("Exit Door")]
    public GameObject exitDoor;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (exitDoor != null)
            exitDoor.SetActive(false);

        UpdateCakeUI();
    }

    public void AddCake()
    {
        collectedCakes++;

        UpdateCakeUI();

        if (collectedCakes >= totalCakesRequired)
        {
            UnlockExitDoor();
        }
    }

    void UpdateCakeUI()
    {
        if (cakeText != null)
            cakeText.text = "Cake: " + collectedCakes + "/" + totalCakesRequired;
    }

    void UnlockExitDoor()
    {
        if (exitDoor != null)
        {
            exitDoor.SetActive(true);
            Debug.Log("Exit door unlocked!");
        }
    }
}
