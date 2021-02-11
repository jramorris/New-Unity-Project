using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SimpleCollectible : Collectible
{
    [SerializeField] UnityEvent OnCollected;
    [SerializeField] float _velocity = 5f;
    [SerializeField] float _collectibleDistance = .5f;
    [SerializeField] float _audioDamper = .5f; // lessen audio output
    [SerializeField] int maxWallHits = 2;

    public override int pointsToGive => 1;
    Vector3 newPosition;
    float width;
    float height;
    int[] choices = new int[] { -1, 1 };
    AudioSource _audioSource;
    float _colliderRadius;
    Light _light;
    Collider2D _collider;
    int wallHits;


    void Awake()
    {
        // adjust velocity after "Instantiated" from pool
        this.OnExitPool += SetVelocityAndTag;
        _audioSource = GetComponent<AudioSource>();
        _colliderRadius = GetComponent<CircleCollider2D>().radius;
        _light = GetComponentInChildren<Light>();
        _collider = GetComponent<Collider2D>();
    }

    protected override void Collect()
    {
        Score.IncrementScore(pointsToGive);
        OnCollected?.Invoke();
        SetInactive();
        Spawner.shouldSpawnCollectible = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        float distance = Vector3.Distance(collision.transform.position, gameObject.transform.position);
        if (distance <= _collectibleDistance)
            Collect();

        _audioSource.volume = ((_colliderRadius * 2 - distance) / (_colliderRadius * 2)) * _audioDamper;
        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
            CheckSetInactive();
    }

    void CheckSetInactive()
    {
        // hack to ignore initial wall hit on spawn
        wallHits++;

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
            Debug.Log(_audioSource.volume);
            yield return null;

        }
        gameObject.SetActive(false);
    }
}
