﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public static bool offMap = false;

    [SerializeField] float _movementSpeed = 5f;
    [SerializeField] float _acceleration = 5f;
    [SerializeField] InputAction moveAction;
    [SerializeField] AudioSource _explosionSound;
    [SerializeField] AudioSource _chargeUpSound;
    [SerializeField] AudioSource _pulseSoundEffect;
    [SerializeField] AudioSource _shieldHitSound;
    [SerializeField] float _maxOffMapTime = 2.5f;
    [SerializeField] Animator _shieldContainerAnim;
    [SerializeField] public int _maxCharge = 10;
    [SerializeField] int _powerToChargeShield = 5;

    // movement
    float horizontal;
    float vertical;

    // health & power
    bool _dead = false;
    int _healthRemaining = 1;
    int _maxHealth = 1;
    int _currentCharge;
    int _shieldCharges;
    int _maxShieldCharges = 2;
    public static event Action<int> OnChargeChange;

    Rigidbody2D _rb;
    Quaternion toQuaternion;
    Animator _playerAnim;
    ParticleSystem _particleSystem;
    float _offMapTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    void FixedUpdate()
    {
        if (!moveAction.enabled)
        {
            // input system doesnt allow enabling in Awake/Start, etc.
            moveAction.Enable();
        }

        if (_dead == true)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        ReadInput();
        
        var newXVelocity = Mathf.Lerp(_rb.velocity.x, horizontal * _movementSpeed, Time.deltaTime * _acceleration);
        var newYVelocity = Mathf.Lerp(_rb.velocity.y, vertical * _movementSpeed, Time.deltaTime * _acceleration);
        _rb.velocity = new Vector2(newXVelocity, newYVelocity);

        if (horizontal != 0 || vertical != 0)
        {
            toQuaternion = Quaternion.FromToRotation(Vector3.up, new Vector3(horizontal, vertical, 0));
            toQuaternion = Quaternion.Euler(new Vector3(0f, 0f, toQuaternion.eulerAngles.z + 90));
            transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, _movementSpeed * Time.deltaTime);
        }
    }

    public void CollectPower()
    {
        if (_currentCharge < _maxCharge)
        {
            _currentCharge++;
            OnChargeChange(_currentCharge);
        }

        if (_currentCharge % _powerToChargeShield == 0)
            _shieldCharges++;

        _chargeUpSound.PlayOneShot(_chargeUpSound.clip, 1f);
    }

    public void ShieldsUp()
    {
        _chargeUpSound.PlayOneShot(_chargeUpSound.clip, 1f);
    }

    private void ReadInput()
    {
        var moveDirection = moveAction.ReadValue<Vector2>();
        horizontal = moveDirection.x;
        vertical = moveDirection.y;

    }

    public void PulseBomb()
    {
        if (_currentCharge == _maxCharge)
        {
            _particleSystem.Play();
            _pulseSoundEffect.PlayDelayed(.1f);
            _currentCharge = 0;
            OnChargeChange(_currentCharge);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.layer == 9)
        {
            other.GetComponent<Renderer>().enabled = false;
            other.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void TakeDamage()
    {
        if (_shieldCharges > 0)
            ShieldHit();
        else
        {
            _healthRemaining--;
            if (_healthRemaining == 0)
                Die();
        }

    }

    private void ShieldHit()
    {
        
        _currentCharge -= _powerToChargeShield;
        OnChargeChange(_currentCharge);
        _shieldCharges--;
        _shieldContainerAnim.SetTrigger("ShieldHit");
        _shieldHitSound.Play();
    }

    void Die()
    {
        _playerAnim.SetTrigger("Explode");
        _explosionSound.Play();
        _dead = true;
        GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().ZeroScore();
        
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
        StartCoroutine("GoToMenu");
    }

    IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {
            offMap = true;
            StartCoroutine("CrashAfterSeconds");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {
            StopCoroutine("CrashAfterSeconds");
            _offMapTime = 0;
            offMap = false;
        }
    }

    IEnumerator CrashAfterSeconds()
    {
        while (_offMapTime < _maxOffMapTime)
        {
            _offMapTime += Time.deltaTime;
            float wiggleDistance = UnityEngine.Random.Range(-.05f, .05f);
            transform.position = new Vector3(transform.position.x + wiggleDistance, transform.position.y + wiggleDistance);
            yield return null;

        }
        Die();
    }
}
