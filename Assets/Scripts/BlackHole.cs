using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] float _gravityForce = 5f;
    [SerializeField] GameObject _prefab;
    [SerializeField] int _spawnEveryInt = 5;

    [SerializeField] int maxImpacts = 5;
    int impacts;

    List<Collider2D> _bodiesToForce = new List<Collider2D>();
    Spawner _spawner;
    GameObject _child;
    ParticleSystem _waveParticles;
    ParticleSystem _novaParticles;
    SpriteRenderer _rippleRenderer;
    Animator _lightAnimator;
    GameObject _childTwo;
    Animator _collisionAnimator;
    Collider2D _childCollider;
    SpriteRenderer _bhRenderer;

    private void Start()
    {
        _spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
    }

    private void Awake()
    {
        _child = transform.GetChild(0).gameObject;
        _childCollider = _child.GetComponent<Collider2D>();
        _waveParticles = _child.GetComponent<ParticleSystem>();
        _collisionAnimator = _child.GetComponent<Animator>();
        _collisionAnimator.SetTrigger("Spawn");
        _bhRenderer = _child.GetComponent<SpriteRenderer>();

        _lightAnimator = transform.GetChild(1).gameObject.GetComponent<Animator>();

        _childTwo = transform.GetChild(2).gameObject;
        _novaParticles = _childTwo.GetComponent<ParticleSystem>();
        _rippleRenderer = _childTwo.GetComponent<SpriteRenderer>();

        _childCollider.enabled = false;
        //_bhRenderer.enabled = false;
        SpawnAnimation();
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

    public void Collapse()
    {
        _bhRenderer.enabled = false;
        _rippleRenderer.enabled = false;
        _novaParticles.Play();
        _childCollider.enabled = false;
        StartCoroutine(InactiveAfterSeconds(2f));
    }

    IEnumerator InactiveAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(.5f);
        _spawner.SpawnSeeker(transform.position);

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

    public void Spawn()
    {
        if (Score.CurrentScore() %  _spawnEveryInt == 0)
        {
            GameObject newObj = Instantiate(_prefab, RandomOnScreen(), Quaternion.identity);
            newObj.GetComponent<BlackHole>().SpawnAnimation();
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
        _childCollider.enabled = true;
        _bhRenderer.enabled = true;
    }

    Vector2 RandomOnScreen()
    {
        // this checks to see if we are spawning the black hole in a place with a collider within the radius of the black hole 10 times and if 
        // we find a spot that doesn't have a collider, spawn the black hole
        var width = UnityEngine.Random.Range(-20, 20);
        var height = UnityEngine.Random.Range(-15, 15);
        var overlap = Physics2D.OverlapCircle(new Vector2(width, height), 2f);
        for (int i = 0; i < 10; i++)
        {
            width = UnityEngine.Random.Range(-20, 20);
            height = UnityEngine.Random.Range(-15, 15);
            overlap = Physics2D.OverlapCircle(new Vector2(width, height), 2f);
            if (overlap == null)
                return new Vector3(width, height, 0);
        }
        return new Vector3(width, height, 0);
    }
}
