using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    public static event Action OnAttack;

    private float timeBetweenMeleeAttack;
    public float startTimeBetweenMeleeAttack;
    public float meleeDamage;
    public float critChance;
    public float critModifier;
    public bool crit;
    public void SetCrit(bool _crit) { crit = _crit; }

    private float timeBetweenProjectile;
    public float startTimeBetweenProjectile;
    public int projectileDamage;
    // can I put the class based projectile in the class object?
    public GameObject Projectile;


    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public FloatVariable attackRange;
    public float defaultAttackRange = 0.4f;
    public float knockback = 1f;

    private void OnEnable()
    {
        PlayerMovement.OnAttack += Attack;
    }

    private void OnDisable()
    {
        PlayerMovement.OnAttack -= Attack;
    }

    void Start()
    {
        attackRange.value = defaultAttackRange;
    }

    void Update()
    {

        /*
        if(timeBetweenMeleeAttack <= 0)
        {
            if (Input.GetButtonDown("attack"))
            {
                // create an array of enemies in the collider area that you can damage
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange.value, whatIsEnemies);
                float damageModifier = CalculateDamageModifier();
                
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //deal damage and knockback
                    Vector2 direction = enemiesToDamage[i].transform.position - transform.position;
                    Vector2 knockbackVector = direction.normalized * knockback;
                    var enemyHealth = enemiesToDamage[i].GetComponent<EnemyHealth>();
                    if(enemyHealth)
                        enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(meleeDamage * damageModifier, knockbackVector);
                }
                timeBetweenMeleeAttack = startTimeBetweenMeleeAttack;
                OnAttack?.Invoke();
            }
        }
        else
            timeBetweenMeleeAttack -= Time.deltaTime;

        */
        if (timeBetweenProjectile <= 0)
        {
            if (Input.GetMouseButtonDown(1))
            {
                SpawnProjectile();
                OnAttack?.Invoke();
            }
        }
        else
            timeBetweenProjectile -= Time.deltaTime;
    }

    private void Attack()
    {
        // create an array of enemies in the collider area that you can damage
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange.value, whatIsEnemies);
        float damageModifier = CalculateDamageModifier();

        for (int i = 0; i < enemiesToDamage.Length; i++)
        {
            //deal damage and knockback
            Vector2 direction = enemiesToDamage[i].transform.position - transform.position;
            Vector2 knockbackVector = direction.normalized * knockback;
            var enemyHealth = enemiesToDamage[i].GetComponent<EnemyHealth>();
            if (enemyHealth)
                enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(meleeDamage * damageModifier, knockbackVector);
        }
        OnAttack?.Invoke();
    }

    private float CalculateDamageModifier()
    {
        float modifier = 1f;

        if (crit)
        {
            modifier *= critModifier;
        }
        return modifier;
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
