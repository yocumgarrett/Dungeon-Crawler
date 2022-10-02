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
    
}
