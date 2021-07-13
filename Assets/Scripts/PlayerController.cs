using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    public static bool offMap = false;

    // audio
    [SerializeField] AudioSource _explosionSound;
    [SerializeField] AudioSource _fullChargeSound;
    [SerializeField] AudioSource _pulseSoundEffect;
    [SerializeField] AudioSource _shieldHitSound;
    [SerializeField] AudioSource _collectChargeSound;
    [SerializeField] AudioClip _speedUpSound;

    [SerializeField] bool _inTutorial;
    [SerializeField] float _maxOffMapTime = 2.5f;
    [SerializeField] Animator _shieldContainerAnim;
    [SerializeField] SpriteRenderer _shieldRenderer;

    [SerializeField] public float _maxPower = 10;
    [SerializeField] public float _maxCharge = 10;
    [SerializeField] public int _maxShieldCharges = 2;
    [SerializeField] float _baseDecrementMultiplier = .5f;
    [SerializeField] public int _powerToChargeShield = 5;

    [SerializeField] TrailRenderer _leftTrail;
    [SerializeField] TrailRenderer _rightTrail;
    [SerializeField] public int _requiredSpeedUpCharge = 2;
    [SerializeField] public int _requiredNovaCharge = 10;

    // movement
    [SerializeField] float _movementSpeed = 5f;
    [SerializeField] float _acceleration = 5f;
    [SerializeField] InputAction moveAction;
    [SerializeField] float _maxMovementModifier = 1.5f;
    public float _movementModifier = 1;
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
    public float _currentCharge;
    int _shieldCharges;

    // emission
    [SerializeField] float maxEmissionIntensity = 0.8f;
    private float currentIntensity;
    private float _desiredIntensity;
    Color _emissionColor = new Color(190, 10, 0);

    // shielding
    bool _shielding;
    Coroutine shieldCoroutine;

    // random stuff
    Rigidbody2D _rb;
    Quaternion toQuaternion;
    Animator _playerAnim;
    ParticleSystem _particleSystem;
    Renderer _spriteRenderer;
    private GameObject _indicatorContainer;
    float _offMapTime;
    float _decrementMultiplier;
    Coroutine _dieCoroutine;
    UIManager _UIManager;
    
    private float wiggleDistance;
    const int CollidableLayer = 9;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerAnim = GetComponent<Animator>();
        _particleSystem = GetComponent<ParticleSystem>();
        _spriteRenderer = gameObject.GetComponent<Renderer>();
        _indicatorContainer = transform.GetChild(0).gameObject;
        _UIManager = GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>();
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
            newXVelocity = Mathf.Lerp(_rb.velocity.x, horizontal * _movementSpeed * _movementModifier, Time.deltaTime * _acceleration);
            newYVelocity = Mathf.Lerp(_rb.velocity.y, vertical * _movementSpeed * _movementModifier, Time.deltaTime * _acceleration);
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
        if (_inTutorial)
            return;

        if (horizontal != 0 || vertical != 0)
            _decrementMultiplier = _baseDecrementMultiplier * 5f;
        else
            _decrementMultiplier = _baseDecrementMultiplier;

        _currentPower -= Time.deltaTime * _decrementMultiplier;
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
        if (_dieCoroutine != null)
            StopCoroutine(_dieCoroutine);

        // power increment
        _currentPower = Mathf.Clamp(_currentPower + 2.5f, 0f, _maxPower);

        // charge increment
        _currentCharge = Mathf.Clamp(_currentCharge + 1f, 0f, _maxCharge);
        SetShipColor();
        OnChargeChange(_currentCharge);


        if (_currentCharge == _maxCharge)
            _fullChargeSound.PlayOneShot(_fullChargeSound.clip, 1f);
        else
            _collectChargeSound.PlayOneShot(_collectChargeSound.clip, 1f);
    }

    public void PulseBomb()
    {
        _particleSystem.Play();
        _pulseSoundEffect.PlayDelayed(.1f);
        ResetPower();
    }

    public void IncreaseMovementSpeed()
    {
        if (_movementModifier < _maxMovementModifier && _currentCharge >= _requiredSpeedUpCharge)
        {
            _collectChargeSound.PlayOneShot(_speedUpSound, 1f);
            _movementModifier += .05f;
            _currentCharge -= _requiredSpeedUpCharge;
            OnChargeChange(_currentCharge);
            SetShipColor();
        }

        // TODOS

        // do nothing if at max speed √
        // do nothing if not enough charge √
        // sound effect √
        // update UI if (other.CompareTag("BlackHole")) ?
        // remove update movement modifier (from event) √
        // motion blur
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.layer == CollidableLayer)
        {
            if (other.CompareTag("SmallAsteroid"))
                other.GetComponent<SmallAsteroid>().BreakUp();

            else if (other.CompareTag("Asteroid"))
                other.GetComponent<RedEnemy>().BreakUp();

            else if (other.CompareTag("Seeker"))
                other.GetComponent<Seeker>().Explode();

            else if (other.CompareTag("BlackHole"))
                other.GetComponent<BHCollisionDetector>().BreakUp();
        }
    }

    public void TakeDamage()
    {
        if (_shielding)
            return;

        _healthRemaining--;
        if (_healthRemaining == 0)
            Explode();
    }

    public void ShieldsUp()
    {
        if (_currentCharge < _powerToChargeShield || _dead == true)
            return;

        // does this need to consider already running routines?
        StartCoroutine(ShieldForSeconds(1));

        _currentCharge -= _powerToChargeShield;
        OnChargeChange(_currentCharge);

        _shieldCharges--;
        SetShipColor();

        _shieldContainerAnim.SetTrigger("ShieldHit");
        _shieldHitSound.Play();
    }

    IEnumerator ShieldForSeconds(int seconds)
    {
        _shielding = true;
        yield return new WaitForSeconds(seconds);
        _shielding = false;
    }

    void Explode()
    {
        if (_dead == true)
            return;

        _playerAnim.SetTrigger("Explode");
        _explosionSound.Play();
        StopTrails();
        transform.localScale = new Vector3(1, 1, transform.localScale.z);
        Die();
    }

    void StopTrails()
    {
        _leftTrail.emitting = false;
        _rightTrail.emitting = false;
    }

    IEnumerator DieAfterSeconds(int numSeconds)
    {
        yield return new WaitForSeconds(numSeconds);
        Die();
    }

    void Die()
    {
        _dead = true;
        _indicatorContainer.SetActive(false);
        StartCoroutine("GoToMenu");
        GameObject.FindGameObjectWithTag("Score").GetComponent<Score>().ZeroScore();
    }

    IEnumerator GoToMenu()
    {
        yield return new WaitForSeconds(2f);
        _UIManager.LaunchDeathInterstitial();
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Map"))
        {
            offMap = true;
            StartCoroutine("CrashAfterSeconds");
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Map"))
        {
            StopCoroutine("CrashAfterSeconds");
            _offMapTime = 0;
            offMap = false;
        }

        if (collider.gameObject.layer == CollidableLayer)
        {
            TakeDamage();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == CollidableLayer)
        {
            TakeDamage();
        }
    }


    IEnumerator CrashAfterSeconds()
    {
        while (_offMapTime < _maxOffMapTime)
        {
            _offMapTime += Time.deltaTime;
            wiggleDistance = UnityEngine.Random.Range(-.05f, .05f);
            transform.position = new Vector3(transform.position.x + wiggleDistance, transform.position.y + wiggleDistance);
            yield return null;

        }
        Explode();
    }

    // set initial game states
    void Initialize()
    {
        _currentPower = _maxPower;
        ResetPower();
    }

    void ResetPower()
    {
        _currentCharge = 0f;
        _shieldCharges = 0;
        SetShipColor();
        OnChargeChange(_currentCharge);
    }

    void SetShipColor()
    {
        if (shieldCoroutine != null)
            StopCoroutine(shieldCoroutine);

        shieldCoroutine = StartCoroutine(UpdateEmissionColor(_currentCharge / _maxCharge));
    }

    IEnumerator UpdateEmissionColor(float intensityMultiplier)
    {
        // ship outline color
        _desiredIntensity = intensityMultiplier * maxEmissionIntensity;
        // for mike 0.1875f(intensityMultiplier * intensityMultiplier) + 0.875(intensityMultiplier);

        while (currentIntensity < _desiredIntensity)
        {
            currentIntensity += _desiredIntensity * .01f;
            _spriteRenderer.material.SetColor("_Color", _emissionColor * currentIntensity);
            yield return null;
        }
        _spriteRenderer.material.SetColor("_Color", _emissionColor * _desiredIntensity);
    }

    public void InvokeChargeChangeEvent()
    {
        // another tutorial hack
        OnChargeChange(_currentCharge);
    }
}
