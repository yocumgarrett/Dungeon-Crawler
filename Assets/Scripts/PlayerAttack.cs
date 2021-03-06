using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBetweenMeleeAttack;
    public float startTimeBetweenMeleeAttack;
    public int meleeDamage;

    private float timeBetweenProjectile;
    public float startTimeBetweenProjectile;
    public int projectileDamage;
    // can I put the class based projectile in the class object?
    public GameObject Projectile;


    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public FloatVariable attackRange;
    public float defaultAttackRange = 0.4f;

    void Start()
    {
        attackRange.value = defaultAttackRange;
    }

    void Update()
    {
        if(timeBetweenMeleeAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // create an array of enemies in the collider area that you can damage
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange.value, whatIsEnemies);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //deal damage
                    var enemyHealth = enemiesToDamage[i].GetComponent<EnemyHealth>();
                    if(enemyHealth)
                        enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(meleeDamage);
                }
                timeBetweenMeleeAttack = startTimeBetweenMeleeAttack;
            }
        }
        else
            timeBetweenMeleeAttack -= Time.deltaTime;

        if (timeBetweenProjectile <= 0)
        {
            if (Input.GetMouseButtonDown(1))
                SpawnProjectile();
        }
        else
            timeBetweenProjectile -= Time.deltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange.value);
    }

    private void SpawnProjectile()
    {
        GameObject spawnedProjectile = Instantiate(Projectile, transform.position, Quaternion.identity);
        var projectile = spawnedProjectile.GetComponent<Projectile>();
        if (projectile)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized;
            projectile.ShootProjectile(direction);
        }

        timeBetweenProjectile = startTimeBetweenProjectile;
    }
}
