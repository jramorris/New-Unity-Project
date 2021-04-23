using UnityEngine;

public class Seeker : MonoBehaviour
{
    [SerializeField] float _rotationMultiplier = 2f;
    [SerializeField] float _movementSpeed = 1f;

    Transform _playerTransform;
    Vector3 relativePosition;
    Quaternion toQuaternion;

    void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        relativePosition = _playerTransform.position - transform.position;
        toQuaternion = Quaternion.FromToRotation(Vector3.up, relativePosition);
        toQuaternion = Quaternion.Euler(new Vector3(0f, 0f, toQuaternion.eulerAngles.z + 90));
        transform.rotation = Quaternion.Slerp(transform.rotation, toQuaternion, Time.deltaTime * _rotationMultiplier);

        transform.position = Vector3.MoveTowards(transform.position, relativePosition, Time.deltaTime * _movementSpeed);
        //Debug.Log($"would be pos: {transform.position + (transform.right * _movementSpeed)}");
        //Debug.Log($"lerped pos: {Vector3.Lerp(transform.position, transform.position + (transform.right * _movementSpeed), Time.deltaTime)}");
        //transform.position += Vector3.Lerp(transform.position, , Time.deltaTime);
    }
}
