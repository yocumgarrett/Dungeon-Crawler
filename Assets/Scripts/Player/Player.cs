using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    enum PlayerState
    {
        Idle,
        Walking,
        Attacking,
        HitStun
    }
    private PlayerState state;

    public static event Action OnPlayerHit;
    public static event Action OnAttack;
    private Animator animator;

    [Header("Movement")]
    public float speed;
    private Vector3 changeInPosition;
    private Rigidbody2D myRigidbody;
    
    [Header("Attack")]
    public float baseCritChance;
    public FloatVariable CritChance;
    public float attackWaitTime;
    private bool crit;

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
    public FloatVariable maxProjectiles;
    public FloatVariable numberProjectilesHeld;
    public float startTimeBetweenProjectile;
    private float timeBetweenProjectile;

    [Header("HitStun")]
    public float hitstunWaitTime;

    [Header("Stats")]
    public FloatVariable MaxHealth;
    public FloatVariable Health;

    [Header("Attributes")]
    public PlayerClass playerClass;

    private void Awake()
    {
        MaxHealth.value = playerClass.health;
        Health.value = playerClass.health;
        CritChance.value = baseCritChance;
    }

    void Start()
    {
        state = PlayerState.Idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        attackRange.value = defaultAttackRange;
        numberProjectilesHeld.value = maxProjectiles.value;
    }

    private void Update()
    {
        changeInPosition = Vector3.zero;
        changeInPosition.x = Input.GetAxisRaw("Horizontal");
        changeInPosition.y = Input.GetAxisRaw("Vertical");
        if(changeInPosition != Vector3.zero && state != PlayerState.Attacking && state != PlayerState.HitStun)
        {
            state = PlayerState.Walking;
        }
        if (Input.GetButtonDown("attack"))
        {
            state = PlayerState.Attacking;
            float critOutcome = UnityEngine.Random.value;
            if (critOutcome >= 0 && critOutcome <= CritChance.value)
                crit = true;
            else
                crit = false;
            Attack();
            StartCoroutine(AttackCo(crit));
            
        }

        if (timeBetweenProjectile <= 0)
        {
            if (Input.GetMouseButtonDown(1) && numberProjectilesHeld.value > 0)
            {
                SpawnProjectile();
                --numberProjectilesHeld.value;
                OnAttack?.Invoke();
            }
        }
        else
            timeBetweenProjectile -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        UpdateAnimationAndMove();
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
        string attack_bool;
        if (crit)
            attack_bool = "attacking-crit";
        else
            attack_bool = "attacking";
        ChooseAttackAnimation();
        animator.SetBool(attack_bool, true);
        yield return null;
        animator.SetBool(attack_bool, false);
        yield return new WaitForSeconds(attackWaitTime);

        state = PlayerState.Idle;
    }

    void ChooseAttackAnimation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized;
        float octagonal_direction = CalculateOctagonalDirection(direction);
        switch (octagonal_direction)
        {
            case 0:
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", 0);
                break;
            case 1:
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", 1);
                break;
            case 2:
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", 1);
                break;
            case 3:
                animator.SetFloat("moveX", -1);
                animator.SetFloat("moveY", 1);
                break;
            case 4:
                animator.SetFloat("moveX", -1);
                animator.SetFloat("moveY", 0);
                break;
            case 5:
                animator.SetFloat("moveX", -1);
                animator.SetFloat("moveY", -1);
                break;
            case 6:
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", -1);
                break;
            case 7:
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", -1);
                break;
            case 8:
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", 0);
                break;
            default:
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", -1);
                break;
        }
    }

    private float CalculateOctagonalDirection(Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        var dir = Mathf.Floor((angle + 22.5f) / 45f);
        return dir;
    }

    private void TakeDamage(float damage)
    {
        StartCoroutine(HitstunCo());
        Health.value -= damage;
        if (Health.value <= 0)
        {
            Destroy(gameObject);
        }
        OnPlayerHit?.Invoke();
    }

    private IEnumerator HitstunCo()
    {
        state = PlayerState.HitStun;
        animator.SetBool("hitstun", true);
        yield return null;
        animator.SetBool("hitstun", false);
        yield return new WaitForSeconds(hitstunWaitTime);
        state = PlayerState.Idle;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
        }
    }

    void UpdateAnimationAndMove()
    {
        if (changeInPosition != Vector3.zero && state != PlayerState.Attacking && state != PlayerState.HitStun)
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
