using UnityEngine;

public class RedEnemy : BaseEnemy
{
    [SerializeField] int _velocityMagnitude = 5;
    [SerializeField] int _smallRockSpawnVelocityMagnifier = 5;
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
            collision.collider.GetComponent<PlayerController>().TakeDamage();
            BreakUp(collision);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("BlackHole"))
        {
            BreakUp();
        }
    }

    public void BreakUp(Collision2D collision = null)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        int childCount = transform.childCount;
        if (collision != null)
        {
            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(0);
                child.gameObject.SetActive(true);
                child.GetComponent<Rigidbody2D>().velocity = collision.contacts[0].normal * (1f + (collision.relativeVelocity.magnitude * .1f));
                child.parent = null;
            }
        }
        _particleSystem.Play();
    }
}
