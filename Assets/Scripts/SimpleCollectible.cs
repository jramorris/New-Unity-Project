using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class SimpleCollectible : Collectible
{
    [SerializeField] UnityEvent OnCollected;
    [SerializeField] float _velocity = 5f;
    [SerializeField] float _collectibleDistance = .5f;
    [SerializeField] float _audioDamper = .5f; // lessen audio output
    [SerializeField] float _audioRadius = 2;
    [SerializeField] int maxWallHits = 2;

    public override int pointsToGive => 1;
    AudioSource _audioSource;
    Light2D _light;
    Collider2D _collider;
    int wallHits;
    GameObject _player;
    SpriteRenderer _spriteRenderer;
    private Coroutine _becomeInactiveCoroutine;
    private Rigidbody2D _rb;
    private ParticleSystem _particles;
    private Vector2 _spawnVelocity;

    void Awake()
    {
        // adjust velocity after "Instantiated" from pool
        this.OnExitPool += Spawn;
        _audioSource = GetComponent<AudioSource>();
        _light = GetComponentInChildren<Light2D>();
        _collider = GetComponent<Collider2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _particles = GetComponent<ParticleSystem>();
        OnCollected.AddListener(_player.GetComponent<PlayerController>().CollectPower);
    }

    private void Update()
    {
        float distance = Vector3.Distance(_player.transform.position, gameObject.transform.position);
        if (_collider.enabled && distance <= _audioRadius)
        {
            _audioSource.volume = ((_audioRadius - distance) / (_audioRadius)) * _audioDamper;
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }
    }

    protected override void Collect()
    {
        _particles.Play();
        SetInactive();
        Score.IncrementScore(pointsToGive);
        OnCollected?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Collect();

        if (collision.CompareTag("Map") && _becomeInactiveCoroutine != null)
        {
            StopCoroutine(_becomeInactiveCoroutine);
            _becomeInactiveCoroutine = null;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Map") && gameObject.activeSelf)
            _becomeInactiveCoroutine = StartCoroutine("BecomeInactive");
    }

    IEnumerator BecomeInactive()
    {
        yield return new WaitForSeconds(2f);
        SetInactive();
    }

    void SetInactive()
    {
        _rb.velocity = Vector2.zero;
        _collider.enabled = false;
        _light.enabled = false;
        _spriteRenderer.enabled = false;
        wallHits = 0;
        Spawner.shouldSpawnCollectible = true;
        StartCoroutine("VolumeToZero");
    }

    private void OnDestroy()
    {
        this.OnExitPool -= Spawn;
    }

    private void Enable()
    {
        _collider.enabled = true;
        _light.enabled = true;
        _spriteRenderer.enabled = true;
        _rb.velocity = _spawnVelocity;
        gameObject.tag = "Collectible";
    }

    void Spawn()
    {
        _collider.enabled = false;
        _light.enabled = false;
        _spriteRenderer.enabled = false;
        _spawnVelocity = _rb.velocity * _velocity;
        _rb.velocity = Vector2.zero;
        _particles.Play();
        StartCoroutine(EnableAfterSeconds(.5f));
    }

    IEnumerator EnableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Enable();

    }

    IEnumerator VolumeToZero()
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= Time.deltaTime;
            yield return null;

        }
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
}
