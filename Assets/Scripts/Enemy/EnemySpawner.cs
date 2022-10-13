using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float timeBetweenSpawns;
    private float spawnTimer = 0f;
    private Vector3 spawnerPosition;
    private bool spawnEnemies;
    public void SetToSpawn(bool to_spawn) { spawnEnemies = to_spawn; }

    void Start()
    {
        spawnerPosition = transform.position;
        SetToSpawn(true);
    }

    void Update()
    {
        if (spawnTimer >= timeBetweenSpawns && spawnEnemies)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
        else
            spawnTimer += Time.deltaTime;
    }

    void SpawnEnemy()
    {
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
    }
}
