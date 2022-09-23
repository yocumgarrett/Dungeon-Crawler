using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    enum EnemyState
    {
        Aggro,
        Idle,
        HitStun,
        Death
    }
    EnemyState state;

    bool has_target;
    bool chase;
    public string target_tag = "Player";
    public float speed = 1f;
    public Rigidbody2D rb;
    public CircleCollider2D bodyCollider;
    public Animator animator;
    private Vector2 moveDirection;
    public float knockbackTimeInSeconds;
    public float deathTimeInSeconds;

    public int minSpawnEnergy;
    public int maxSpawnEnergy;
    public GameObject Energy;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CircleCollider2D>();
        state = EnemyState.Aggro;
        has_target = true;
        chase = true;
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (has_target && state == EnemyState.Aggro)
        {
            ChasePlayer();
            UpdateAnimation();
        }
        else
            Idle();
    }

    private void ChasePlayer()
    {
        GameObject target = GameObject.FindGameObjectWithTag(target_tag);
        if (target)
        {
            MoveTowardsTarget(target.transform.position);
        }
    }

    void UpdateAnimation()
    {
        if(moveDirection != Vector2.zero)
        {
            var angle = CalculateDirectionAngle(moveDirection);
            if (angle >= 247.5 && angle < 292.5)
            {
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", -1);
            }
            else if (angle >= 292.5 && angle < 337.5)
            {
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", -1);
            }
            else if (angle >= 337.5 || angle < 22.5)
            {
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", 0);
            }
            else if (angle >= 22.5 && angle < 67.5)
            {
                animator.SetFloat("moveX", 1);
                animator.SetFloat("moveY", 1);
            }
            else if (angle >= 67.5 && angle < 112.5)
            {
                animator.SetFloat("moveX", 0);
                animator.SetFloat("moveY", 1);
            }
            else if (angle >= 112.5 && angle < 157.5)
            {
                animator.SetFloat("moveX", -1);
                animator.SetFloat("moveY", 1);
            }
            else if (angle >= 157.5 && angle < 202.5)
            {
                animator.SetFloat("moveX", -1);
                animator.SetFloat("moveY", 0);
            }
            else if (angle >= 202.5 && angle < 247.5)
            {
                animator.SetFloat("moveX", -1);
                animator.SetFloat("moveY", -1);
            }
        }
    }

    private void Idle()
    {

    }

    public void TookDamage(Vector2 knockback)
    {
        rb.AddForce(knockback);
        StartCoroutine(HitStunCoroutine());
    }

    IEnumerator HitStunCoroutine()
    {
        state = EnemyState.HitStun;
        animator.SetBool("took_damage", true);
        yield return null;
        animator.SetBool("took_damage", false);
        yield return new WaitForSeconds(knockbackTimeInSeconds);
        state = EnemyState.Aggro;        
    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        has_target = false;
        rb.velocity = Vector2.zero;
        bodyCollider.enabled = false;
        Vector3 deathPosition = transform.parent.transform.position;
        SpawnEnergyOnDeath(deathPosition);
        state = EnemyState.Death;
        animator.SetBool("die", true);
        yield return new WaitForSeconds(deathTimeInSeconds);
        //add phase out and instantiate head to kick around
        Destroy(transform.parent.gameObject);
    }

    public void SpawnEnergyOnDeath(Vector3 pos)
    {
        var numToSpawn = Random.Range(minSpawnEnergy, maxSpawnEnergy);
        for (var i = 0; i < numToSpawn; i++)
        {
            GameObject toSpawn = Instantiate(Energy, pos, Quaternion.identity);
        }
    }

    public void MoveTowardsTarget(Vector2 targetPosition)
    {
        moveDirection = new Vector2(targetPosition.x - transform.parent.transform.position.x, targetPosition.y - transform.parent.transform.position.y);

        if (rb)
            rb.velocity = moveDirection * speed * Time.deltaTime;

    }

    private float CalculateDirectionAngle(Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        return angle;
    }
}
