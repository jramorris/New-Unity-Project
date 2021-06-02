using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : PooledMonoBehavior
{
    [SerializeField] float _gravityForce = 5f;
    [SerializeField] int maxImpacts = 5;
    int impacts;

    List<Collider2D> _bodiesToForce = new List<Collider2D>();
    Spawner _spawner;
    GameObject _child;
    Collider2D[] _childColliders;
    ParticleSystem _waveParticles;
    ParticleSystem _novaParticles;
    SpriteRenderer _rippleRenderer;
    Animator _lightAnimator;
    GameObject _childTwo;
    Animator _collisionAnimator;
    SpriteRenderer _bhRenderer;

    private void Awake()
    {
        _child = transform.GetChild(0).gameObject;
        _childColliders = _child.GetComponents<Collider2D>();
        _waveParticles = _child.GetComponent<ParticleSystem>();
        _collisionAnimator = _child.GetComponent<Animator>();
        _bhRenderer = _child.GetComponent<SpriteRenderer>();

        _lightAnimator = transform.GetChild(1).gameObject.GetComponent<Animator>();

        _childTwo = transform.GetChild(2).gameObject;
        _novaParticles = _childTwo.GetComponent<ParticleSystem>();
        _rippleRenderer = _childTwo.GetComponent<SpriteRenderer>();
        _spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        this.OnExitPool += Spawn;
    }

    void OnDestroy()
    {
        this.OnExitPool -= Spawn;
    }

    public void Spawn()
    {
        SpawnAnimation();
        _collisionAnimator.SetTrigger("Spawn");
        foreach (Collider2D _childCollider in _childColliders)
            _childCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        _bodiesToForce.Add(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _bodiesToForce.Remove(collision);
    }

    public void IncrementImpacts(int numImpacts)
    {
        impacts += numImpacts;
        _lightAnimator.SetTrigger("Impact");

        if (impacts >= maxImpacts)
            Collapse();
    }

    public void Collapse(bool spawnSeeker = true)
    {
        _bhRenderer.enabled = false;
        _rippleRenderer.enabled = false;
        _novaParticles.Play();
        foreach (Collider2D _childCollider in _childColliders)
            _childCollider.enabled = false;
        StartCoroutine(InactiveAfterSeconds(spawnSeeker));
    }

    IEnumerator InactiveAfterSeconds(bool spawnSeeker)
    {
        if (spawnSeeker == true)
        {
            yield return new WaitForSeconds(.5f);
            _spawner.SpawnSeeker(transform.position);
        }

        while (_novaParticles.isPlaying == true)
            yield return null;

        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        foreach (Collider2D collision in _bodiesToForce)
        {
            var distance = Vector3.Distance(gameObject.transform.position, collision.transform.position);
            var normalizedDistance = distance < 1 ? 1 : distance;
            var gravityFactor = 1 / (normalizedDistance * normalizedDistance);
            collision.attachedRigidbody?.AddForce((transform.position - collision.transform.position) * (_gravityForce * gravityFactor));
        }
    }

    void SpawnAnimation()
    {
        _waveParticles.Play();
        StartCoroutine(EnableAfterSeconds(.8f));
    }

    IEnumerator EnableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        foreach (Collider2D _childCollider in _childColliders)
            _childCollider.enabled = true;
        _bhRenderer.enabled = true;
    }
}
