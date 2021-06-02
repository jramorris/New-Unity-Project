using UnityEngine;

public class BHCollisionDetector : MonoBehaviour
{
    BlackHole blackHoleScript;

    const int CollidableLayer = 9;

    private void Awake()
    {
        blackHoleScript = transform.parent.GetComponent<BlackHole>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerController>().TakeDamage();

        else if (collision.gameObject.layer == CollidableLayer)
            blackHoleScript.IncrementImpacts(1);
    }

    public void BreakUp()
    {
        blackHoleScript.Collapse(false);
    }
}
