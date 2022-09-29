using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerHit;
    public static event Action OnAttack;
    private Animator animator;

    [Header("Movement")]
    public float speed;
    private Vector3 changeInPosition;
    private Rigidbody2D myRigidbody;
    
    [Header("Attack")]
    public float critChance;
    public float attackWaitTime;
    private bool crit;
    private bool attacking = false;

    private float timeBetweenMeleeAttack;
    public float startTimeBetweenMeleeAttack;
    public float meleeDamage;
    public float critModifier;

    public Transform attackPos;
    public LayerMask whatIsEnemies;
    public LayerMask whatIsBreakable;
    public FloatVariable attackRange;
    public float defaultAttackRange = 0.4f;
    public float knockback = 1f;

    [Header("Projectile")]
    public GameObject Projectile;
    public int projectileDamage;
    public int maxProjectiles;
    private int numberProjectilesHeld;
    public float startTimeBetweenProjectile;
    private float timeBetweenProjectile;

    [Header("Stats")]
    public FloatVariable MaxHealth, Health;

    [Header("Attributes")]
    public PlayerClass playerClass;

    private void Awake()
    {
        MaxHealth.value = playerClass.health;
        Health.value = playerClass.health;
    }

    void Start()
    {
        
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackRange.value = defaultAttackRange;
        numberProjectilesHeld = maxProjectiles;
    }

    private void Update()
    {
        changeInPosition = Vector3.zero;
        changeInPosition.x = Input.GetAxisRaw("Horizontal");
        changeInPosition.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack"))
        {
            
            attacking = true;
            float critOutcome = UnityEngine.Random.value;
            if (critOutcome >= 0 && critOutcome <= critChance)
                crit = true;
            else
                crit = false;
            Attack();
            StartCoroutine(AttackCo(crit));
            
        }

        if (timeBetweenProjectile <= 0)
        {
            if (Input.GetMouseButtonDown(1) && numberProjectilesHeld > 0)
            {
                SpawnProjectile();
                --numberProjectilesHeld;
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
        Collider2D[] breakablesToHit = Physics2D.OverlapCircleAll(attackPos.position, attackRange.value, whatIsBreakable);
        float damageModifier = CalculateDamageModifier();
        for (int i = 0; i < breakablesToHit.Length; i++)
        {
            var destroyable = breakablesToHit[i].GetComponent<DestroyableObject>();
            if (destroyable)
                breakablesToHit[i].GetComponent<DestroyableObject>().TakeHit();
        }
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

    private IEnumerator AttackCo(bool crit)
    {
        if (crit)
        {
            animator.SetBool("attacking-crit", true);
            yield return null;
            animator.SetBool("attacking-crit", false);
            yield return new WaitForSeconds(attackWaitTime);
        }
        else
        {
            animator.SetBool("attacking", true);
            yield return null;
            animator.SetBool("attacking", false);
            yield return new WaitForSeconds(attackWaitTime);
        }
        attacking = false;
    }

    private void FixedUpdate()
    {
        UpdateAnimationAndMove();
    }

    private void TakeDamage(float damage)
    {
        Health.value -= damage;
        if (Health.value <= 0)
        {
            Destroy(gameObject);
        }
        OnPlayerHit?.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(20);
        }
    }

    void UpdateAnimationAndMove()
    {
        if (changeInPosition != Vector3.zero && !attacking)
        {
            MovePlayer();
            animator.SetFloat("moveX", changeInPosition.x);
            animator.SetFloat("moveY", changeInPosition.y);
        }
    }

    private void MovePlayer()
    {
        myRigidbody.MovePosition(
             transform.position + changeInPosition * speed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange.value);
    }
}
