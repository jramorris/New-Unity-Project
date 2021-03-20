using UnityEngine;

public class RedEnemy : BaseEnemy
{
    [SerializeField] int _velocityMagnitude = 5;
    [SerializeField] GameObject enemyPrefab;

    ParticleSystem _particleSystem;

    void Awake()
    {
        // adjust velocity after "Instantiated" from pool
        this.OnExitPool += SetVelocity;
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnDestroy()
    {
        this.OnExitPool -= SetVelocity;
    }

    private void SetVelocity()
    {
        Rigidbody2D _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _rb.velocity * _velocityMagnitude;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player"))
        {
            BreakUp();
            collision.collider.GetComponent<PlayerController>().TakeDamage();
        }
    }

    void BreakUp()
    {
        _particleSystem.Play();
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            child.parent = null;
        }
    }
}
