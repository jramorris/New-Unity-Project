using System;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    [SerializeField] float _rotationMultiplier = 2f;
    [SerializeField] float _movementSpeed = 1f;

    Transform _playerTransform;
    Rigidbody2D _rb;
    Vector3 relativePosition;
    Quaternion toQuaternion;

    void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        relativePosition = _playerTransform.position - transform.position;
        toQuaternion = Quaternion.FromToRotation(Vector3.up, relativePosition);
        toQuaternion = Quaternion.Euler(new Vector3(0f, 0f, toQuaternion.eulerAngles.z + 90));
        transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, Time.deltaTime * _rotationMultiplier);

        _rb.velocity = transform.right * _movementSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    void Explode()
    {
        gameObject.SetActive(false);
    }
}
