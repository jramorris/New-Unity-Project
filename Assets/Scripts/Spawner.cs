using UnityEngine;

public class Spawner : MonoBehaviour
{
    // spawn timers
    float _spawnTimer;
    float _spawnWaitTime = 2f;

    [SerializeField] bool pauseSpawns;

    // spawn vars
    int[] choices = new int[] { -1, 1 };
    float width;
    float height;
    Vector2 randomizer;
    private PooledMonoBehavior newObj;
    public static bool shouldSpawnCollectible;

    [SerializeField] PooledMonoBehavior asteroidPrefab;
    [SerializeField] PooledMonoBehavior smallAsteroidPrefab;
    [SerializeField] PooledMonoBehavior collectiblePrefab;
    [SerializeField] GameObject seekerPrefab;

    float _velocityMultiplier;
    private Vector2 spawnPosition;
    private Vector2 spawnVelocity;

    void Update()
    {
        if (pauseSpawns)
            return;

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
        return _spawnTimer > (_spawnWaitTime - (1.8f - (1.8f / ((Score.CurrentScore() * .25f) + 1f))));
    }

    PooledMonoBehavior Spawn(PooledMonoBehavior objectPrefab, Vector2? position = null, Vector2? velocity = null)
    {
        _spawnTimer = 0f;
        _spawnWaitTime = UnityEngine.Random.Range(2f, 4f);

        if (position == null)
            spawnPosition = RandomOnScreenEdge();
        else
            spawnPosition = (Vector2)position;

        if (velocity == null)
            spawnVelocity = new Vector2(spawnPosition.x > 0 ? -1f : 1f, spawnPosition.y > 0 ? -1f : 1f);
        else
            spawnVelocity = (Vector2)velocity;

        // asteroids faster with higher score
        _velocityMultiplier = asteroidPrefab == objectPrefab ? Random.Range(1, 1 + (0.05f * Score.CurrentScore() * .25f)) : 1;
        return objectPrefab.Get<PooledMonoBehavior>(spawnPosition,
                                                    Quaternion.identity,
                                                    spawnVelocity * _velocityMultiplier);
    }

    public void SpawnSmallAsteroid(Vector2 position, Collision2D collision = null)
    {
        randomizer = Random.Range(0, 2) > 0 ? new Vector2(0, Random.Range(-2, 3)) : new Vector2(Random.Range(-2, 3), 0);
        newObj = Spawn(smallAsteroidPrefab,
                       position,
                       randomizer + (collision.contacts[0].normal * (1f + (collision.relativeVelocity.magnitude * .1f))));
    }

    Vector2 RandomOnScreenEdge()
    {
        Vector2 mapSize = GameObject.FindGameObjectWithTag("Map").GetComponent<SpriteRenderer>().bounds.size;
        float mapWidth = (mapSize.x / 2) * 1.2f;
        float mapHeight = (mapSize.y / 2) * 1.2f;
        int randomInt = Random.Range(0, 2);

        if (randomInt > 0)
        {
            // sides
            randomInt = Random.Range(0, 2);
            width = mapWidth * choices[randomInt];
            height = UnityEngine.Random.Range(-mapHeight, mapHeight);
        }
        else
        {
            // top | bottom
            randomInt = Random.Range(0, 2);
            width = UnityEngine.Random.Range(-mapWidth, mapWidth);
            height = mapHeight * choices[randomInt];
        }
        return new Vector3(width, height, 0);
    }

    public void SpawnSeeker(Vector3 position)
    {
        Instantiate(seekerPrefab, position, Quaternion.identity);
    }
}
