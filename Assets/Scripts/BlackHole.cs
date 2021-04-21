using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] float _gravityForce = 5f;
    [SerializeField] GameObject _prefab;
    [SerializeField] int _spawnEveryInt = 5;

    [SerializeField] int maxImpacts = 5;
    int impacts;

    List<Collider2D> _bodiesToForce = new List<Collider2D>();

    void OnTriggerEnter2D(Collider2D collision)
    {
        _bodiesToForce.Add(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        _bodiesToForce.Remove(collision);
    }

    public void IncrementImpacts(int numImpacts)
    {
        impacts += numImpacts;

        if (impacts >= maxImpacts)
            Collapse();
    }

    void Collapse()
    {
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        foreach (Collider2D collision in _bodiesToForce)
        {
            var distance = Vector3.Distance(gameObject.transform.position, collision.transform.position);
            var normalizedDistance = distance < 1 ? 1 : distance;
            var gravityFactor = 1 / (normalizedDistance * normalizedDistance);
            collision.attachedRigidbody?.AddForce((transform.position - collision.transform.position) * (_gravityForce * gravityFactor));
        }
    }

    public void Spawn()
    {
        if (Score.CurrentScore() %  _spawnEveryInt == 0)
            Instantiate(_prefab, RandomOnScreen(), Quaternion.identity);
    }

    Vector2 RandomOnScreen()
    {
        // this checks to see if we are spawning the black hole in a place with a collider within the radius of the black hole 10 times and if 
        // we find a spot that doesn't have a collider, spawn the black hole
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
