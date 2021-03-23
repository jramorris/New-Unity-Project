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
            collision.collider.GetComponent<PlayerController>().TakeDamage();
            BreakUp();
        }
    }

    void BreakUp()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var child = transform.GetChild(0);
            Debug.Log($"loop {child.gameObject.name}");
            child.gameObject.SetActive(true);
            child.parent = null;
        }
        //Debug.Log("ended loop");
        //Transform[] children = GetComponentsInChildren<Transform>();
        //foreach (Transform child in children)
        //{
        //    Debug.Log($"loop {child.gameObject.name}");
        //    child.gameObject.SetActive(true);
        //    child.parent = null;
        //}
        //_particleSystem.Play();
    }
}
