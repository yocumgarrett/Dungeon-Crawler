using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public int minSpawnEnergy;
    public int maxSpawnEnergy;
    public GameObject Energy;

    private void Update()
    {
        if (health <= 0)
        {
            Vector3 deathPosition = transform.parent.transform.position;
            Destroy(transform.parent.gameObject);

            SpawnEnergyOnDeath(deathPosition);
        }
    }

    public void TakeDamage(float damage, Vector2 knockback)
    {
        health -= damage;
        var enemyAI = GetComponentInParent<EnemyAI>();
        if (enemyAI)
        {

            enemyAI.rb.AddForce(knockback);
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
