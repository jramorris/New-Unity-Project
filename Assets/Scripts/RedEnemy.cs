using System.Collections;
using UnityEngine;

public class RedEnemy : BaseEnemy
{
    [SerializeField] int _velocityMagnitude = 5;
    [SerializeField] int _smallRockSpawnVelocityMagnifier = 5;
    [SerializeField] int numAsteroidsToSpawn = 2;
    [SerializeField] GameObject enemyPrefab;


    ParticleSystem _asteroidParticles;
    ParticleSystem _lightParticles;
    ParticleSystem _spawnParticles;
    SpriteRenderer _spriteRenderer;
    CircleCollider2D _collider;
    Rigidbody2D _rb;
    private AudioSource _audio;
    Spawner _spawner;
    ManageIndicators indicatorPanel;
    Vector2 spawnVelocity;
    float timePassed;
    const int CollidableLayer = 9;

    void Awake()
    {
        // adjust velocity after "Instantiated" from pool
        this.OnExitPool += Spawn;
        _lightParticles = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
        _spawnParticles = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();
        _asteroidParticles = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _audio = GetComponent<AudioSource>();
        _spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();

        // all indicator related, functionality that's not being used
        //this.OnExitPool += NotifyIndicatorManager;
        //this.OnReturnToPool += RemoveFromIndicator;
        //indicatorPanel = GameObject.FindGameObjectWithTag("IndicatorPanel").GetComponent<ManageIndicators>();
    }

    void OnDestroy()
    {
        this.OnExitPool -= Spawn;
        // all indicator related, functionality that's not being used
        //this.OnExitPool -= NotifyIndicatorManager;
        //this.OnReturnToPool -= RemoveFromIndicator;
    }

    void Spawn()
    {
        spawnVelocity = _rb.velocity;
        _rb.velocity = Vector2.zero;
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _spawnParticles.GetComponent<ParticleSystem>().Play();
        StartCoroutine(StartAfterSeconds(.8f));
    }

    IEnumerator StartAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _spriteRenderer.enabled = true;
        _collider.enabled = true;
        StartCoroutine("SetVelocity");
    }

    IEnumerator SetVelocity()
    {
        timePassed = 0;
        while (_rb.velocity != (spawnVelocity * _velocityMagnitude))
        {
            timePassed += Time.deltaTime;
            _rb.velocity = Vector2.Lerp(_rb.velocity, (spawnVelocity * _velocityMagnitude), timePassed * 0.08f);
            yield return null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BreakUp();
        for (int i = 0; i < numAsteroidsToSpawn; i++)
            _spawner.SpawnSmallAsteroid(transform.position, collision);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == CollidableLayer)
        {
            BreakUp();
        }
    }

    public void BreakUp()
    {
        _audio.Play();
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _asteroidParticles.Play();
        _lightParticles.Play();
    }

    void NotifyIndicatorManager()
    {
        // add self to asteroid list on exit queue
        indicatorPanel.asteroids.Add(transform);
    }

    void RemoveFromIndicator(PooledMonoBehavior obj)
    {
        // A better workflow would be to have manager listen for OnExitPool?
        indicatorPanel.asteroids.Remove(transform);
    }
}
