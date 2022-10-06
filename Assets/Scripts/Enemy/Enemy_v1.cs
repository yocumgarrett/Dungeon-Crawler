using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float knockbackTimeInSeconds;
    public float deathTimeInSeconds;

    void Start()
    {
        SetState(EnemyState.Idle);
    }

    void Update()
    {
        
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
        //SpawnEnergyOnDeath(deathPosition);
        //SpawnProjectileOnDeath(deathPosition);
        myAnimator.SetBool("die", true);
        yield return new WaitForSeconds(deathTimeInSeconds);
        //add phase out and instantiate head to kick around
        Destroy(transform.parent.gameObject);
    }
}
