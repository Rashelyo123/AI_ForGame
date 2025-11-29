using UnityEngine;

public class Cake : MonoBehaviour
{
    public float flashTime = 0.15f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddCake();

            StartCoroutine(PickupEffect());
        }
    }

    System.Collections.IEnumerator PickupEffect()
    {

        sr.color = Color.white;


        yield return new WaitForSeconds(flashTime);


        Destroy(gameObject);
    }
}
