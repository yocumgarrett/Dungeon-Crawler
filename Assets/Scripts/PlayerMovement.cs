using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerClass playerClass;

    private Vector2 moveDirection;

    // Update is called once per frame
    void Update()
    {
        // Process Inputs
        ProcessInputs();
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
        rb.velocity = playerClass.speed * new Vector2(moveDirection.x, moveDirection.y);
    }
}
