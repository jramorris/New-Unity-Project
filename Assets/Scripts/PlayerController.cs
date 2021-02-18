using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public static bool offMap = false;

    [SerializeField] float _movementSpeed = 5f;
    [SerializeField] float _acceleration = 5f;
    [SerializeField] InputAction moveAction;
    [SerializeField] InputActionAsset playerControls;
    [SerializeField] AudioSource _explosionSound;
    [SerializeField] AudioSource _shieldChargeSound;
    [SerializeField] AudioSource _pulseSoundEffect;
    [SerializeField] float _maxOffMapTime = 2.5f;

    InputAction move;
    Rigidbody2D _rb;
    Quaternion toQuaternion;
    Animator _anim;
    ParticleSystem _particleSystem;
    bool _dead = false;
    int _healthRemaining = 1;
    int maxHealth = 1;
    float horizontal;
    float vertical;
    float _offMapTime;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        move.Disable();
    }

    void FixedUpdate()
    {
        if (move == null)
        {
            // input system doesnt allow enabling in Awake/Start, etc.
            move = playerControls.FindActionMap("Player").FindAction("Move");
            move.Enable();
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

    public void ChargeShield()
    {
        _shieldChargeSound.PlayOneShot(_shieldChargeSound.clip, 1f);
    }

    public void ShieldsUp()
    {
        _shieldChargeSound.PlayOneShot(_shieldChargeSound.clip, 1f);
    }

    private void ReadInput()
    {
        var moveDirection = moveAction.ReadValue<Vector2>();
        horizontal = moveDirection.x;
        vertical = moveDirection.y;
    }

    public void PulseBomb()
    {
        _particleSystem.Play();
        _pulseSoundEffect.PlayDelayed(.1f);
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
        _healthRemaining--;
        if (_healthRemaining == 0)
            Die();
    }

    void Die()
    {
        _anim.SetTrigger("Explode");
        _explosionSound.Play();
        _dead = true;
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
            float wiggleDistance = Random.Range(-.05f, .05f);
            transform.position = new Vector3(transform.position.x + wiggleDistance, transform.position.y + wiggleDistance);
            yield return null;

        }
        Die();
    }
}
