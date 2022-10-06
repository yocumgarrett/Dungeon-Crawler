using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_v1 : MonoBehaviour
{
    enum PlayerState
    {
        Idle,
        Move,
        Attack,
        HitStun
    }
    private PlayerState playerState;
    private void SetState(PlayerState state) { playerState = state; }
    private Animator animator;

    [Header("Movement")]
    public float moveSpeed;
    private Vector3 moveInputVector = Vector3.zero;
    public Rigidbody2D myRigidbody;

    [Header("Attack")]
    public float attackWaitTime;
    private bool attackInput = false;
    private float attackCooldownTime;
    public Transform attackCenterPoint;
    public FloatVariable attackRadius;
    public LayerMask enemyLayer;
    public LayerMask breakableObjectLayer;
    public float knockbackMagnitude = 1f;
    public float attackDamage;

    public FloatVariable PlayerCritChance;
    public float criticalDamageModifier;

    [Header("Projectile")]
    public GameObject Projectile;
    public float shootWaitTime;
    private float shootCooldownTime = 0f;
    private float numberProjectilesHeld;
    public FloatVariable MaxProjectiles;


    [Header("HitStun")]
    public float hitStunWaitTime;
    private float hitStunCooldownTime;

    [Header("Health")]
    public FloatVariable Health;

    void Start()
    {
        SetState(PlayerState.Idle);
        animator = GetComponent<Animator>();
        numberProjectilesHeld = MaxProjectiles.value;
    }

    void Update()
    {
        
        if (shootCooldownTime <= 0)
        {
            bool shootInput = Input.GetMouseButtonDown(1);
            if (shootInput && numberProjectilesHeld > 0)
            {
                Shoot();
                shootCooldownTime = shootWaitTime;
            }
        }
        else
            shootCooldownTime -= Time.deltaTime;

        if (attackCooldownTime <= 0 && hitStunCooldownTime <= 0)
        {
            moveInputVector.x = Input.GetAxisRaw("Horizontal");
            moveInputVector.y = Input.GetAxisRaw("Vertical");
            attackInput = Input.GetButtonDown("attack");

            if (attackInput)
            {
                SetState(PlayerState.Attack);
                UpdateAnimationAndAttack();
                attackCooldownTime = attackWaitTime;
            }
            else if (moveInputVector != Vector3.zero && (playerState == PlayerState.Idle || playerState == PlayerState.Move))
            {
                SetState(PlayerState.Move);
            }
            else
                SetState(PlayerState.Idle);
        }
        else
        {
            attackCooldownTime -= Time.deltaTime;
            hitStunCooldownTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if(playerState == PlayerState.Move)
        {
            UpdateAnimationAndMove();
        }
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
        myRigidbody.MovePosition(transform.position + moveInputVector * moveSpeed * Time.deltaTime);
        animator.SetFloat("moveX", moveInputVector.x);
        animator.SetFloat("moveY", moveInputVector.y);
    }

    void UpdateAnimationAndAttack()
    {
        bool crit = Crit();
        StartCoroutine(AttackAnimationCoroutine(crit));
        float damageModifier = CalculateDamageModifier(crit);
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackCenterPoint.position, attackRadius.value, enemyLayer);
        
        for (int enemy = 0; enemy < enemiesToDamage.Length; enemy++)
        {
            Vector2 dir_kb = enemiesToDamage[enemy].transform.position - transform.position;
            Vector2 knockbackVector = dir_kb.normalized * knockbackMagnitude;
            var enemyScript = enemiesToDamage[enemy].GetComponent<Enemy_v1>();
            if (enemyScript)
               enemyScript.TakeDamage(attackDamage * damageModifier, knockbackVector, this.gameObject);
        }

        Collider2D[] breakablesToHit = Physics2D.OverlapCircleAll(attackCenterPoint.position, attackRadius.value, breakableObjectLayer);

        for (int obj = 0; obj < breakablesToHit.Length; obj++)
        {
            var breakable_obj = breakablesToHit[obj].GetComponent<DestroyableObject>();
            if (breakable_obj)
                breakablesToHit[obj].GetComponent<DestroyableObject>().TakeHit();
        }
    }

    private bool Crit()
    {
        bool crit;
        float crit_outcome = UnityEngine.Random.value;
        if (crit_outcome >= 0 && crit_outcome <= PlayerCritChance.value)
            crit = true;
        else
            crit = false;
        return crit;
    }

    private float CalculateDamageModifier(bool crit)
    {
        float modifier = 1f;
        if (crit)
            modifier *= criticalDamageModifier;
        return modifier;
    }

    private IEnumerator AttackAnimationCoroutine(bool crit)
    {
        SwitchAttackAnimation();
        string attack_bool;
        if (crit)
            attack_bool = "attacking-crit";
        else
            attack_bool = "attacking";
        animator.SetBool(attack_bool, true);
        yield return null;
        animator.SetBool(attack_bool, false);
        yield return new WaitForSeconds(attackWaitTime);
    }

    void SwitchAttackAnimation()
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
        StartCoroutine(HitStunCoroutine());
        Health.value -= damage;
        GameManager.Instance.HealthChanged();
        if (Health.value <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator HitStunCoroutine()
    {
        animator.SetBool("hitstun", true);
        yield return null;
        animator.SetBool("hitstun", false);
        yield return new WaitForSeconds(hitStunWaitTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCenterPoint.position, attackRadius.value);
    }

    private void Shoot()
    {
        --numberProjectilesHeld;

        GameObject spawnedProjectile = Instantiate(Projectile, transform.position, Quaternion.identity);
        var projectileScript = spawnedProjectile.GetComponent<Projectile>();

        if (projectileScript)
        {
            Vector3 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = new Vector3(mouse_pos.x - transform.position.x, mouse_pos.y - transform.position.y, 0).normalized;
            projectileScript.ShootProjectile(dir);
        }
    }
}
