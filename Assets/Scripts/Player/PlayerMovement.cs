using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerClass playerClass;
    public float speedScalar;

    private Vector2 moveDirection;
    Vector3 mousePos;
    Vector3 scaleVector;

    private void Awake()
    {
        scaleVector = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Process Inputs
        ProcessInputs();

        Flip();
    }

    // FixedUpdate called before each internal physics update 
    private void FixedUpdate()
    {
        // Physics Calculations   
        Move();
    }

    private void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    private void Move()
    {
        rb.velocity = speedScalar * playerClass.speed * new Vector2(moveDirection.x, moveDirection.y);
    }

    private void Flip()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var horizontalDirection = mousePos.x - transform.position.x;

        if (horizontalDirection >= 0)
            gameObject.transform.localScale = new Vector3(Mathf.Abs(scaleVector.x), scaleVector.y, scaleVector.z);
        
        else if(horizontalDirection <= 0)
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(scaleVector.x), scaleVector.y, scaleVector.z);
    }
}
