using UnityEngine;

public class ManageIndicators : MonoBehaviour
{
    [SerializeField] UnityEngine.Camera mainCamera;
    [SerializeField] Transform _playerTransform;

    void LateUpdate()
    {
        var asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        Debug.Log(asteroids.Length);
        foreach(GameObject asteroid in asteroids)
        {
            //Vector3 screenPosition = mainCamera.WorldToViewportPoint(transform.position);

            Debug.DrawRay(_playerTransform.position, asteroid.transform.position - _playerTransform.position, Color.green);
            //Debug.Log($"Screen dims: {Screen.width} x {Screen.height}");
            //Debug.Log($"Cam dims: {mainCamera.pixelWidth} x {mainCamera.pixelHeight}");
            //Debug.Log($"Obj Screen Pos: {screenPosition}");
            //if (screenPosition.x > mainCamera.pixelWidth || screenPosition.x < 0 ||
            //    screenPosition.y > mainCamera.pixelHeight || screenPosition.y < 0)
            //{
            //    Debug.Log($"screen pos: {screenPosition}");
            //}
        }
    }
}
