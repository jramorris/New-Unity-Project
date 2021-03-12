using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] float _gravityForce = 5f;
    [SerializeField] GameObject _prefab;

    List<Collider2D> _bodiesToForce = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _bodiesToForce.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _bodiesToForce.Remove(collision);
    }

    private void FixedUpdate()
    {
        foreach (Collider2D collision in _bodiesToForce)
        {
            var distance = Vector3.Distance(gameObject.transform.position, collision.transform.position);
            var normalizedDistance = distance < 1 ? 1 : distance;
            var gravityFactor = 1 / (normalizedDistance * normalizedDistance);
            collision.attachedRigidbody?.AddForce((transform.position - collision.transform.position) * (_gravityForce * gravityFactor));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            Debug.Log("Damage, Captain");
    }

    public void Spawn()
    {
        if (Score.CurrentScore() %  1 == 0)
            Instantiate(_prefab, RandomOnScreen(), Quaternion.identity);
    }

    Vector2 RandomOnScreen()
    {
        var width = UnityEngine.Random.Range(-20, 20);
        var height = UnityEngine.Random.Range(-15, 15);
        var overlap = Physics2D.OverlapCircle(new Vector2(width, height), 2f);
        for (int i = 0; i < 10; i++)
        {
            width = UnityEngine.Random.Range(-20, 20);
            height = UnityEngine.Random.Range(-15, 15);
            overlap = Physics2D.OverlapCircle(new Vector2(width, height), 2f);
            if (overlap == null)
                return new Vector3(width, height, 0);
        }
        return new Vector3(width, height, 0);
    }
}
