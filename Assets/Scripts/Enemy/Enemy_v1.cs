using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_v1 : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Aggro,
        Attack,
        Shoot,
        HitStun,
        Death
    }
    public EnemyState enemyState;
    public void SetState(EnemyState state) { enemyState = state; }
    public EnemyState GetState() { return enemyState; }

    [Header("Move")]
    public float chaseSpeed;
    public Rigidbody2D myRigidbody;
    private GameObject chaseTarget;
    public void SetTarget(GameObject target) { chaseTarget = target; }

    [Header("Animation")]
    public Animator myAnimator;
    public CircleCollider2D myCollider;
    public CircleCollider2D detectionCollider;

    [Header("Health")]
    public float health;
    public float maxHealth;
    public Slider healthSlider;
    public GameObject healthBarUI;
    public float knockbackTimeInSeconds;
    public float deathTimeInSeconds;

    [Header("Projectile")]
    public GameObject Projectile;
    public float shootWaitTime;
    private float shootCooldownTime = 0f;
    public float shootCoroutineTime;

    void Start()
    {
        SetState(EnemyState.Idle);
        health = maxHealth;
        UpdateHealthSlider();
    }

    private void Update()
    {
        if (enemyState == EnemyState.Aggro)
        {
            if (shootCooldownTime <= 0)
            {
                StartCoroutine(ShootCoroutine());
                shootCooldownTime = shootWaitTime;
            }
            else
                shootCooldownTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (enemyState == EnemyState.Aggro)
        {
            ChaseTarget();
        }
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (myRigidbody.velocity != Vector2.zero)
        {
            float octagonal_direction = CalculateOctagonalDirection(myRigidbody.velocity);
            switch (octagonal_direction)
            {
                case 0:
                    myAnimator.SetFloat("moveX", 1);
                    myAnimator.SetFloat("moveY", 0);
                    break;
                case 1:
                    myAnimator.SetFloat("moveX", 1);
                    myAnimator.SetFloat("moveY", 1);
                    break;
                case 2:
                    myAnimator.SetFloat("moveX", 0);
                    myAnimator.SetFloat("moveY", 1);
                    break;
                case 3:
                    myAnimator.SetFloat("moveX", -1);
                    myAnimator.SetFloat("moveY", 1);
                    break;
                case 4:
                    myAnimator.SetFloat("moveX", -1);
                    myAnimator.SetFloat("moveY", 0);
                    break;
                case 5:
                    myAnimator.SetFloat("moveX", -1);
                    myAnimator.SetFloat("moveY", -1);
                    break;
                case 6:
                    myAnimator.SetFloat("moveX", 0);
                    myAnimator.SetFloat("moveY", -1);
                    break;
                case 7:
                    myAnimator.SetFloat("moveX", 1);
                    myAnimator.SetFloat("moveY", -1);
                    break;
                case 8:
                    myAnimator.SetFloat("moveX", 1);
                    myAnimator.SetFloat("moveY", 0);
                    break;
                default:
                    myAnimator.SetFloat("moveX", 0);
                    myAnimator.SetFloat("moveY", -1);
                    break;
            }
        }
    }

    private void ChaseTarget()
    {
        if (chaseTarget)
        {
            var target_pos = chaseTarget.transform.position;
            var parent_pos = transform.parent.transform.position;
            var move_dir = new Vector2(target_pos.x - parent_pos.x, target_pos.y - parent_pos.y);
            if (myRigidbody)
                myRigidbody.velocity = move_dir * chaseSpeed * Time.deltaTime;
        }
    }

    private float CalculateOctagonalDirection(Vector3 move_dir)
    {
        var angle = Mathf.Atan2(move_dir.y, move_dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        var dir = Mathf.Floor((angle + 22.5f) / 45f);
        return dir;
    }

    public void TakeDamage(float damage, Vector2 knockbackVector, GameObject target)
    {
        health -= damage;
        UpdateHealthSlider();
        if (health <= 0)
        {
            GameManager.Instance.EnemyKill(this.gameObject.tag, transform.parent.transform.position);
            StartCoroutine(DieCoroutine());
        }
        else
        {
            myRigidbody.AddForce(knockbackVector);
            StartCoroutine(HitStunCoroutine(target));
        }
    }

    IEnumerator HitStunCoroutine(GameObject target)
    {
        SetState(EnemyState.HitStun);
        myAnimator.SetBool("took_damage", true);
        yield return null;
        myAnimator.SetBool("took_damage", false);
        yield return new WaitForSeconds(knockbackTimeInSeconds);
        SetState(EnemyState.Aggro);
        SetTarget(target);
    }

    IEnumerator DieCoroutine()
    {
        SetState(EnemyState.Death);
        myCollider.enabled = false;
        detectionCollider.enabled = false;
        myRigidbody.velocity = Vector2.zero;
        Vector3 death_pos = transform.parent.transform.position;
        myAnimator.SetBool("die", true);
        yield return new WaitForSeconds(deathTimeInSeconds);
        //add phase out and instantiate head to kick around
        Destroy(transform.parent.gameObject);
    }

    IEnumerator ShootCoroutine()
    {
        SetState(EnemyState.Shoot);
        myRigidbody.velocity = Vector2.zero;
        yield return new WaitForSeconds(shootCoroutineTime);
        if(enemyState == EnemyState.Shoot)
        {
            Shoot();
            SetState(EnemyState.Idle);
        }
    }

    private void Shoot()
    {
        GameObject spawnedProjectile = Instantiate(Projectile, transform.position, Quaternion.identity);
        var projectileScript = spawnedProjectile.GetComponent<Projectile>();

        if (projectileScript && chaseTarget)
        {
            var target_pos = chaseTarget.transform.position;
            var parent_pos = transform.parent.transform.position;
            var dir = new Vector2(target_pos.x - parent_pos.x, target_pos.y - parent_pos.y);
            projectileScript.ShootProjectile(dir);
        }
    }

    private void UpdateHealthSlider()
    {
        if (health < maxHealth) healthBarUI.SetActive(true);
        healthSlider.value = health / maxHealth;
    }
}
