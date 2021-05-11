using System.Collections;
using UnityEngine;

public class SmallAsteroid : BaseEnemy
{
    SpriteRenderer _spriteRenderer;
    Collider2D _collider;
    ParticleSystem _particles;

    const int CollidableLayer = 9;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _particles = GetComponent<ParticleSystem>();
        _collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        _collider.enabled = false;
        Debug.Log(gameObject.GetComponent<Rigidbody2D>().velocity);
        StartCoroutine("EnableCollider");
    }

    IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(.5f);
        _collider.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        BreakUp();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == CollidableLayer)
        {
            BreakUp();
        }
    }

    public void BreakUp()
    {
        _spriteRenderer.enabled = false;
        _collider.enabled = false;
        _particles.Play();
    }
}
