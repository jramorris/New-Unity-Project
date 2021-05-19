using UnityEngine;

public class BHCollisionDetector : MonoBehaviour
{
    BlackHole blackHoleScript;

    const int CollidableLayer = 9;

    private void Awake()
    {
        blackHoleScript = GetComponentInParent<BlackHole>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerController>().TakeDamage();

        else if (collision.gameObject.layer == CollidableLayer)
            blackHoleScript.IncrementImpacts(1);
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("bh collide");
    }

    private void OnParticleTrigger()
    {
        Debug.Log("part trig");
    }

    public void BreakUp()
    {
        blackHoleScript.Collapse();
    }
}
