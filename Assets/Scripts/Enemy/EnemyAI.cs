using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    public FloatVariable PlayerMaxProjectiles;
    public FloatVariable PlayerCurrentProjectiles;
    public GameObject[] Powerups;

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
            float octagonal_direction = CalculateOctagonalDirection(moveDirection);
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
        SpawnProjectileOnDeath(deathPosition);
        state = EnemyState.Death;
        animator.SetBool("die", true);
        yield return new WaitForSeconds(deathTimeInSeconds);
        //add phase out and instantiate head to kick around
        Destroy(transform.parent.gameObject);
    }

    public void SpawnEnergyOnDeath(Vector3 pos)
    {
        var numToSpawn = UnityEngine.Random.Range(minSpawnEnergy, maxSpawnEnergy);
        for (var i = 0; i < numToSpawn; i++)
        {
            GameObject toSpawn = Instantiate(Energy, pos, Quaternion.identity);
        }
    }

    public void SpawnProjectileOnDeath(Vector3 pos)
    {
        var chance_to_spawn = ( PlayerMaxProjectiles.value - PlayerCurrentProjectiles.value ) / ( PlayerMaxProjectiles.value * 2f );
        var outcome = UnityEngine.Random.value;
        if(outcome <= chance_to_spawn)
        {
            GameObject toSpawn = Instantiate(Powerups[1], pos, Quaternion.identity);
        }
    }

    public void MoveTowardsTarget(Vector2 targetPosition)
    {
        moveDirection = new Vector2(targetPosition.x - transform.parent.transform.position.x, targetPosition.y - transform.parent.transform.position.y);

        if (rb)
            rb.velocity = moveDirection * speed * Time.deltaTime;

    }

    private float CalculateOctagonalDirection(Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        var dir = Mathf.Floor((angle + 22.5f) / 45f);
        return dir;
    }
}
