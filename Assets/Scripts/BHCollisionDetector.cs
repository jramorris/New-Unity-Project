using UnityEngine;

public class BHCollisionDetector : MonoBehaviour
{
    BlackHole blackHoleScript;

    private void Awake()
    {
        blackHoleScript = GetComponentInParent<BlackHole>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerController>().TakeDamage();

        if (collision.CompareTag("Asteroid"))
            blackHoleScript.IncrementImpacts(1);
    }
}
