using UnityEngine;

public class BaseEnemy : PooledMonoBehavior
{
    int wallHits;
    [SerializeField] int maxWallHits = 2;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerController>().TakeDamage();

        if (collision.CompareTag("Wall"))
            CheckSetInactive();
    }

    void CheckSetInactive()
    {
        // hack to ignore initial wall hit on spawn
        wallHits++;

        if (wallHits >= maxWallHits)
        {
            Invoke("SetInactive", .5f);
        }
    }

    void SetInactive()
    {
        gameObject.SetActive(false);
        wallHits = 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            collision.collider.GetComponent<PlayerController>().TakeDamage();
    }
}
