using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public float timeBetweenSpawns;
    private float spawnTimer = 0f;
    private Vector3 spawnerPosition;

    void Start()
    {
        spawnerPosition = transform.position;
    }

    void Update()
    {
        if (spawnTimer >= timeBetweenSpawns)
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
