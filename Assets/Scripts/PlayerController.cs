using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public static bool offMap = false;

    [SerializeField] AudioSource _explosionSound;
    [SerializeField] AudioSource _chargeUpSound;
    [SerializeField] AudioSource _pulseSoundEffect;
    [SerializeField] AudioSource _shieldHitSound;
    [SerializeField] AudioSource _shieldsUpSound;
    [SerializeField] AudioClip _fullyChargedSound;

    [SerializeField] float _movementSpeed = 5f;
    [SerializeField] float _acceleration = 5f;
    [SerializeField] InputAction moveAction;
    [SerializeField] float _maxOffMapTime = 2.5f;
    [SerializeField] Animator _shieldContainerAnim;

    [SerializeField] public float _maxPower = 10;
    [SerializeField] public float _maxCharge = 10;
    [SerializeField] public int _maxShieldCharges = 2;
    [SerializeField] float _baseDecrementMultiplier = .5f;
    [SerializeField] int _powerToChargeShield = 5;

    // movement
    float horizontal;
    float vertical;

    // health
    bool _dead = false;
    int _healthRemaining = 1;
    int _maxHealth = 1;

    // power (fly ship) & charge (shield, pulse)
    public static event Action<float> OnCollectPower;
    public static event Action<float> OnChargeChange;
    public static event Action OnFullCharge;
    public float _currentPower;
    float _currentCharge;
    int _shieldCharges;

    Rigidbody2D _rb;
    Quaternion toQuaternion;
    Animator _playerAnim;
    ParticleSystem _particleSystem;
    Renderer _spriteRenderer;
    float _offMapTime;

    Color _shieldColor = new Color(0, 181, 195);
    Coroutine shieldCoroutine;
    float _decrementMutliplier;
    Coroutine _dieCoroutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
        _particleSystem = GetComponent<ParticleSystem>();
        _spriteRenderer = gameObject.GetComponent<Renderer>();
    }

    private void Start()
    {
        Initialize();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void Update()
    {
        ReadInput();
        DecrementPower();

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
    }

    void FixedUpdate()
    {
        float newXVelocity;
        float newYVelocity;
        if (_currentPower > 0f)
        {
            newXVelocity = Mathf.Lerp(_rb.velocity.x, horizontal * _movementSpeed, Time.deltaTime * _acceleration);
            newYVelocity = Mathf.Lerp(_rb.velocity.y, vertical * _movementSpeed, Time.deltaTime * _acceleration);
        } else
        {
            newXVelocity = Mathf.Lerp(_rb.velocity.x, 0, Time.deltaTime * .5f);
            newYVelocity = Mathf.Lerp(_rb.velocity.y, 0, Time.deltaTime * .5f);
        }
        
        _rb.velocity = new Vector2(newXVelocity, newYVelocity);

        if (horizontal != 0 || vertical != 0)
        {
            toQuaternion = Quaternion.FromToRotation(Vector3.up, new Vector3(horizontal, vertical, 0));
            toQuaternion = Quaternion.Euler(new Vector3(0f, 0f, toQuaternion.eulerAngles.z + 90));
            transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, _movementSpeed * Time.deltaTime);
        }
    }

    private void DecrementPower()
    {
        if (horizontal > 0 || vertical < 0)
            _decrementMutliplier = _baseDecrementMultiplier * 5f;
        else
            _decrementMutliplier = _baseDecrementMultiplier;

        _currentPower -= Time.deltaTime * _decrementMutliplier;
        if (_currentPower < 0)
        {
            _currentPower = 0;
            if (_dieCoroutine == null)
            {
                _dieCoroutine = StartCoroutine(DieAfterSeconds(1));
            }
        }
    }

    void ReadInput()
    {
        var moveDirection = moveAction.ReadValue<Vector2>();
        horizontal = moveDirection.x;
        vertical = moveDirection.y;
    }

    public void CollectPower()
    {
        // power increment
        _currentPower = Mathf.Clamp(_currentPower + 2.5f, 0f, _maxPower);

        // charge increment
        _currentCharge = Mathf.Clamp(_currentCharge + 1f, 0f, _maxCharge);
        OnChargeChange(_currentCharge);

        // TODO update to account for floats here?
        if (_currentCharge % _powerToChargeShield == 0 && _shieldCharges < _maxShieldCharges)
            ShieldsUp();
        else
        {
            _chargeUpSound.PlayOneShot(_chargeUpSound.clip, 1f);
            OnFullCharge();
        }
    }

    public void ShieldsUp()
    {
        _shieldCharges++;
        if (_shieldCharges == _maxShieldCharges)
            _shieldsUpSound.PlayOneShot(_fullyChargedSound, 1f);
        else
            _shieldsUpSound.PlayOneShot(_shieldsUpSound.clip, 1f);
        SetShieldColor();
    }

    public void PulseBomb()
    {
        if (_currentCharge == _maxCharge)
        {
            _particleSystem.Play();
            _pulseSoundEffect.PlayDelayed(.1f);
            ResetPower();
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
                Explode();
        }

    }

    private void ShieldHit()
    {
        _currentCharge -= _powerToChargeShield;
        OnChargeChange(_currentCharge);

        _shieldCharges--;
        SetShieldColor();

        _shieldContainerAnim.SetTrigger("ShieldHit");
        _shieldHitSound.Play();
    }

    void Explode()
    {
        _playerAnim.SetTrigger("Explode");
        _explosionSound.Play();
        _dead = true;
        GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().ZeroScore();
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
        _dead = true;
        Die();
    }

    IEnumerator DieAfterSeconds(int numSeconds)
    {
        yield return new WaitForSeconds(numSeconds);
        Die();
    }

    void Die()
    {
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
        Explode();
    }

    // set initial game states
    void Initialize()
    {
        ResetPower();
    }

    void ResetPower()
    {
        _currentPower = _maxPower;
        _currentCharge = 0;
        _shieldCharges = 0;
        OnCollectPower(_currentCharge);
        OnChargeChange(_currentCharge);
        SetShieldColor();
    }

    private void SetShieldColor()
    {
        // Found through trial and error
        if (_shieldCharges == 0)
            shieldCoroutine = StartCoroutine(ChargeShield(0f));
        else if (_shieldCharges == 1)
            shieldCoroutine = StartCoroutine(ChargeShield(.01f));
        else
            shieldCoroutine = StartCoroutine(ChargeShield(.1f));

    }

    IEnumerator ChargeShield(float intensityMultiplier)
    {
        float currentIntensity = 0f;

        while (currentIntensity < intensityMultiplier)
        {
            currentIntensity += intensityMultiplier * .01f;
            _spriteRenderer.material.SetColor("_Color", _shieldColor * currentIntensity);
            yield return null;
        }
        _spriteRenderer.material.SetColor("_Color", _shieldColor * intensityMultiplier);
    }
}
