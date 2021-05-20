using UnityEngine;

public class Spawner : MonoBehaviour
{
    // spawn timers
    float _spawnTimer;
    float _spawnWaitTime = 2f;

    [SerializeField] bool pauseSpawns;

    // spawn vars
    int[] choices = new int[] { -1, 1 };
    float xPos;
    float yPos;
    Vector2 randomizer;
    PooledMonoBehavior _currentCollectible;
    public static bool shouldSpawnCollectible;

    [SerializeField] PooledMonoBehavior asteroidPrefab;
    [SerializeField] PooledMonoBehavior smallAsteroidPrefab;
    [SerializeField] PooledMonoBehavior collectiblePrefab;
    [SerializeField] PooledMonoBehavior blackHolePrefab;
    [SerializeField] GameObject seekerPrefab;

    float _velocityMultiplier;
    private Vector2 spawnPosition;

    // spawn func vars
    [SerializeField] int _spawnEveryInt = 5;
    Vector2 mapSize;
    float mapWidth;
    float mapHeight;
    float offEdgeWidth;
    float offEdgeHeight;
    Collider2D overlap;


    void Awake()
    {
        mapSize = GameObject.FindGameObjectWithTag("Map").GetComponent<SpriteRenderer>().bounds.size;
        mapWidth = (mapSize.x / 2);
        mapHeight = (mapSize.y / 2);
        offEdgeWidth = mapWidth * 1.2f;
        offEdgeHeight = mapHeight * 1.2f;
    }

    void Update()
    {
        if (pauseSpawns)
            return;

        _spawnTimer += Time.deltaTime;

        if (shouldSpawnAsteroid())
            SpawnAsteroid();

        if (shouldSpawnCollectible)
        {
            // verify there isn't an active collectible
            if (_currentCollectible == null || !_currentCollectible.isActiveAndEnabled)
            {
                shouldSpawnCollectible = false;
                _currentCollectible = SpawnCollectible();
            }
        }
    }

    public void SpawnAsteroid()
    {
        _spawnTimer = 0f;
        _spawnWaitTime = UnityEngine.Random.Range(2f, 4f);

        spawnPosition = RandomOnScreenEdge();
        _velocityMultiplier = Random.Range(1, 1 + (0.05f * Score.CurrentScore() * .25f));
        Spawn(asteroidPrefab,
              spawnPosition,
              new Vector2(spawnPosition.x > 0 ? -1f : 1f, spawnPosition.y > 0 ? -1f : 1f) * _velocityMultiplier);
    }

    PooledMonoBehavior SpawnCollectible()
    {
        return Spawn(collectiblePrefab,
                     spawnPosition,
                     new Vector2(spawnPosition.x > 0 ? -1f : 1f, spawnPosition.y > 0 ? -1f : 1f));
    }

    private bool shouldSpawnAsteroid()
    {
        return _spawnTimer > (_spawnWaitTime - (1.8f - (1.8f / ((Score.CurrentScore() * .25f) + 1f))));
    }

    PooledMonoBehavior Spawn(PooledMonoBehavior objectPrefab, Vector2 position, Vector2? velocity = null)
    {
        return objectPrefab.Get<PooledMonoBehavior>(position,
                                                    Quaternion.identity,
                                                    velocity);
    }

    public void SpawnSmallAsteroid(Vector2 position, Collision2D collision = null)
    {
        randomizer = Random.Range(0, 2) > 0 ? new Vector2(0, Random.Range(-2, 3)) : new Vector2(Random.Range(-2, 3), 0);
        Spawn(smallAsteroidPrefab,
              position,
              randomizer + (collision.contacts[0].normal * (1f + (collision.relativeVelocity.magnitude * .1f))));
    }

    public void SpawnBlackHole()
    {
        if (Score.CurrentScore() % _spawnEveryInt == 0)
        {
            Spawn(blackHolePrefab,
                  RandomOnScreen(),
                  null);
        }
    }

    Vector2 RandomOnScreenEdge()
    {
        // spawn just off map
        int randomInt = Random.Range(0, 2);
        if (randomInt > 0)
        {
            // sides
            randomInt = Random.Range(0, 2);
            xPos = offEdgeWidth * choices[randomInt];
            yPos = UnityEngine.Random.Range(-offEdgeHeight, offEdgeHeight);
        }
        else
        {
            // top | bottom
            randomInt = Random.Range(0, 2);
            xPos = UnityEngine.Random.Range(-offEdgeWidth, offEdgeWidth);
            yPos = offEdgeHeight * choices[randomInt];
        }
        return new Vector2(xPos, yPos);
    }

    Vector2 RandomOnScreen()
    {
        // spawn in map
        // this checks to see if we are spawning the black hole in a place with a collider within the radius of the black hole 10 times and if 
        // we find a spot that doesn't have a collider, spawn the black hole
        for (int i = 0; i < 10; i++)
        {
            Debug.Log($"width: {xPos}; height: {yPos}");
            overlap = Physics2D.OverlapCircle(new Vector2(xPos, yPos), 2f);
            if (overlap == null)
                return new Vector2(xPos, yPos);
        }
        return new Vector2(xPos, yPos);
    }

    public void SpawnSeeker(Vector3 position)
    {
        Instantiate(seekerPrefab, position, Quaternion.identity);
    }
}
