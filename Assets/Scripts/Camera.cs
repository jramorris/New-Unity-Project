using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Transform _playerTransform;
    [SerializeField] float _smoothTime = .3f;

    Transform _transform;
    Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    void Update()
    {
        var desiredPosition = new Vector3(_playerTransform.position.x, _playerTransform.position.y, -10);
        GetComponent<Transform>().position = desiredPosition;
        // Vector3.SmoothDamp(_transform.position, desiredPosition, ref _velocity, _smoothTime);
    }
}
