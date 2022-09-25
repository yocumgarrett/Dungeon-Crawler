using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public static event Action OnAttack;

    public float speed;
    private Vector3 change;
    private Rigidbody2D myRigidbody;
    private Animator animator;
    private bool crit;
    private bool attacking = false;

    public float critChance;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("attack"))
        {
            var attack = GetComponent<PlayerAttack>();
            if (attack)
            {
                attacking = true;
                float critOutcome = UnityEngine.Random.value;
                if (critOutcome >= 0 && critOutcome <= critChance)
                    crit = true;
                else
                    crit = false;
                attack.SetCrit(crit);
                OnAttack?.Invoke();
                StartCoroutine(AttackCo(crit));
            }
        }
    }

    private IEnumerator AttackCo(bool crit)
    {
        if (crit)
        {
            animator.SetBool("attacking-crit", true);
            yield return null;
            animator.SetBool("attacking-crit", false);
            yield return new WaitForSeconds(.30f);
        }
        else
        {
            animator.SetBool("attacking", true);
            yield return null;
            animator.SetBool("attacking", false);
            yield return new WaitForSeconds(.30f);
        }
        attacking = false;
    }

    private void FixedUpdate()
    {
        UpdateAnimationAndMove();
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero && !attacking)
        {
            MovePlayer();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            // set up when I have walk/run animation
            //animator.SetBool("moving", true);
        }
        else
        {
            //animator.SetBool("moving", false);
        }
    }

    private void MovePlayer()
    {
        myRigidbody.MovePosition(
             transform.position + change * speed * Time.deltaTime);
    }
}
