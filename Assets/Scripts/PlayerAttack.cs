using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private float timeBetweenAttack;
    public float startTimeBetweenAttack;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public FloatVariable attackRange;
    public float defaultAttackRange = 0.4f;
    public int damage;

    void Start()
    {
        attackRange.value = defaultAttackRange;
    }

    void Update()
    {
        if(timeBetweenAttack <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // create an array of enemies in the collider area that you can damage
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange.value, whatIsEnemies);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    //deal damage
                    enemiesToDamage[i].GetComponent<EnemyHealth>().TakeDamage(damage);
                }
                timeBetweenAttack = startTimeBetweenAttack;
            }
        }
        else
        {
            timeBetweenAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange.value);
    }
}
