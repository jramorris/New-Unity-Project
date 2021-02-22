using System.Collections;
using UnityEngine;
using UnityEngine.Events;


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
    Light _light;
    Collider2D _collider;
    int wallHits;
    GameObject _player;

    void Awake()
    {
        // adjust velocity after "Instantiated" from pool
        this.OnExitPool += SetVelocityAndTag;
        _audioSource = GetComponent<AudioSource>();
        _light = GetComponentInChildren<Light>();
        _collider = GetComponent<Collider2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
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
        SetInactive();
        Score.IncrementScore(pointsToGive);
        OnCollected?.Invoke();
        Spawner.shouldSpawnCollectible = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Collect();

        if (collision.CompareTag("Wall"))
            CheckSetInactive();
    }

    void CheckSetInactive()
    {
        // hack to ignore initial wall hit on spawn
        wallHits++;
        Debug.Log($"Hit {wallHits}");
        if (wallHits >= maxWallHits)
        {
            Invoke("SetInactive", 2f);
        }
    }

    void SetInactive()
    {
        _collider.enabled = false;
        _light.enabled = false;
        StartCoroutine("VolumeToZero");
        wallHits = 0;
        Spawner.shouldSpawnCollectible = true;
    }

    private void OnDestroy()
    {
        this.OnExitPool -= SetVelocityAndTag;
    }

    private void SetVelocityAndTag()
    {
        Rigidbody2D _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _rb.velocity * _velocity;
        gameObject.tag = "Collectible";
    }

    IEnumerator VolumeToZero()
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume -= Time.deltaTime;
            yield return null;

        }
        gameObject.SetActive(false);
    }
}
