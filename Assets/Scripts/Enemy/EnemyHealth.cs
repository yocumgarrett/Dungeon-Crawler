using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public int minSpawnEnergy;
    public int maxSpawnEnergy;
    public GameObject Energy;

    public void TakeDamage(float damage, Vector2 knockback)
    {
        health -= damage;
        var enemyAI = GetComponent<EnemyAI>();
        if (enemyAI)
        {
            if (health <= 0)
                enemyAI.Die();
            else
                enemyAI.TookDamage(knockback);
        }
    }

    public void SpawnEnergyOnDeath(Vector3 pos)
    {
        var numToSpawn = Random.Range(minSpawnEnergy, maxSpawnEnergy);
        for (var i = 0; i < numToSpawn; i++)
        {
            GameObject toSpawn = Instantiate(Energy, pos, Quaternion.identity);
        }
    }

    
}
