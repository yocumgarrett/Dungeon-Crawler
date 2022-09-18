using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector3 change;
    private Rigidbody2D myRigidbody;
    private Animator animator;

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
            StartCoroutine(AttackCo());
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.30f);
    }

    private void FixedUpdate()
    {
        UpdateAnimationAndMove();
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
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
