using UnityEngine;

public class Spawner : MonoBehaviour
{
    // spawn timers
    float _spawnTimer;
    float _spawnWaitTime = 2f;

    // spawn vars
    int[] choices = new int[] { -1, 1 };
    float width;
    float height;
    public static bool shouldSpawnCollectible;

    [SerializeField] PooledMonoBehavior asteroidPrefab;
    [SerializeField] PooledMonoBehavior collectiblePrefab;
    GameObject _scoreTextObject;

    private void Start()
    {
        _scoreTextObject = GameObject.FindGameObjectWithTag("ScoreText");
    }

    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (shouldSpawnAsteroid())
            Spawn(asteroidPrefab);

        if (shouldSpawnCollectible)
        {
            shouldSpawnCollectible = false;
            Spawn(collectiblePrefab);
        }
    }

    private bool shouldSpawnAsteroid()
    {
        return _spawnTimer > _spawnWaitTime;
    }

    void Spawn(PooledMonoBehavior objectPrefab)
    {
        _spawnTimer = 0f;
        _spawnWaitTime = UnityEngine.Random.Range(2f, 4f);

        var newPosition = RandomOnScreenEdge();
        var xVelocity = newPosition.x > 0 ? -1f : 1f;
        var yVelocity = newPosition.y > 0 ? -1f : 1f;
        objectPrefab.Get<PooledMonoBehavior>(newPosition,
                                             Quaternion.identity,
                                             new Vector2(xVelocity, yVelocity));
    }

    Vector2 RandomOnScreenEdge()
    {
        Vector2 bgSize = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>().bounds.size;
        int randomInt = Random.Range(0, 2);

        if (randomInt > 0)
        {
            // sides
            randomInt = Random.Range(0, 2);
            width = (bgSize.x / 2) * choices[randomInt];
            height = UnityEngine.Random.Range(-(bgSize.y / 2), (bgSize.y / 2));
        }
        else
        {
            // top | bottom
            randomInt = Random.Range(0, 2);
            width = UnityEngine.Random.Range(-(bgSize.x / 2), (bgSize.x / 2));
            height = (bgSize.y / 2) * choices[randomInt];
        }
        return new Vector3(width, height, 0);
    }
}
