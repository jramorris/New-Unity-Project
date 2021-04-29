﻿using UnityEngine;

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
    public static bool shouldSpawnCollectible;

    [SerializeField] PooledMonoBehavior asteroidPrefab;
    [SerializeField] PooledMonoBehavior collectiblePrefab;
    [SerializeField] GameObject seekerPrefab;

    float _velocityMultiplier;

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

    void Spawn(PooledMonoBehavior objectPrefab)
    {
        _spawnTimer = 0f;
        _spawnWaitTime = UnityEngine.Random.Range(2f, 4f);

        var newPosition = RandomOnScreenEdge();
        var xVelocity = newPosition.x > 0 ? -1f : 1f;
        var yVelocity = newPosition.y > 0 ? -1f : 1f;
        // asteroids faster with higher score
        _velocityMultiplier = asteroidPrefab == objectPrefab ? Random.Range(1, 1 + (0.05f * Score.CurrentScore() * .25f)) : 1;
        objectPrefab.Get<PooledMonoBehavior>(newPosition,
                                             Quaternion.identity,
                                             new Vector2(xVelocity, yVelocity) * _velocityMultiplier);
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
