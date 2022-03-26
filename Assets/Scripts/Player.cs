using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float current_health;
    public float moveSpeed;

    [Header("Attributes")]
    public PlayerClass playerClass;

    [Header("Movement")]
    public Rigidbody2D rb;
    public float move_coefficient;
    public float dodge_coefficient;

    private void Awake()
    {
        current_health = playerClass.health;
        moveSpeed = playerClass.speed * move_coefficient;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
        //Dodge();
    }

    private void Move()
    {
        // manipulate rigidbody 2D velocity
        var x_direction = Input.GetAxisRaw("Horizontal");
        var y_direction = Input.GetAxisRaw("Vertical");

        rb.velocity = moveSpeed * Time.deltaTime * new Vector2(x_direction, y_direction).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var current_velocity = rb.velocity;
            rb.velocity *= dodge_coefficient;
        }
    }

    private void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var current_velocity = rb.velocity;
            rb.velocity *= dodge_coefficient;
        }
    }
}
