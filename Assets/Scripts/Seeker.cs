﻿using System.Collections;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    [SerializeField] float _rotationMultiplier = 2f;
    [SerializeField] float _movementSpeed = 1f;

    Transform _playerTransform;
    Rigidbody2D _rb;
    private AudioSource _audioSource;
    ParticleSystem _particleSystem;
    SpriteRenderer _spriteRenderer;
    Collider2D _collider;
    Transform _orb;
    Vector3 relativePosition;
    Quaternion toQuaternion;
    bool _dead;
    Color tmpColor;

    void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _particleSystem = GetComponent<ParticleSystem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _orb = transform.GetChild(0);
        _particleSystem.Play();
    }

    void FixedUpdate()
    {
        if (!_dead)
        {
            relativePosition = _playerTransform.position - transform.position;
            toQuaternion = Quaternion.FromToRotation(Vector3.up, relativePosition);
            toQuaternion = Quaternion.Euler(new Vector3(0f, 0f, toQuaternion.eulerAngles.z + 90));
            transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, Time.deltaTime * _rotationMultiplier);

            _rb.velocity = transform.right * _movementSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    public void Explode()
    {
        _dead = true;
        _audioSource.Play();
        _particleSystem.Play();
        _collider.enabled = false;
        _orb.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //_spriteRenderer.enabled = false;
        StartCoroutine(InactiveAfterSeconds());
    }

    IEnumerator InactiveAfterSeconds()
    {
        tmpColor = _spriteRenderer.color;
        while (tmpColor.a > 0)
        {
            tmpColor.a -= Time.deltaTime;
            _spriteRenderer.color = tmpColor;
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
}
