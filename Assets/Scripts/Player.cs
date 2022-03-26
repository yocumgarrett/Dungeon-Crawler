using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float max_health;
    public float current_health;
    public float speed;
    public float stamina;
    public float power;
    public float poise;
    public float guard;

    public float GetHealth() { return max_health; }
    public float GetSpeed() { return speed; }
    public float GetStamina() { return stamina; }
    public float GetPower() { return power; }
    public float GetPoise() { return poise; }
    public float GetGuard() { return guard; }

    [Header("Attributes")]
    public PlayerClass playerClass;
    public string passive;
    public string skill1;
    public string skill2;

    [Header("Movement")]
    public Rigidbody2D rb;
    public float move_coefficient;
    public float dodge_coefficient;

    public void SetPlayerClass(PlayerClass playerClass)
    {
        max_health = playerClass.health;
        speed = playerClass.speed;
        stamina = playerClass.stamina;
        power = playerClass.power;
        poise = playerClass.poise;
        guard = playerClass.guard;

        passive = playerClass.passive;
        skill1 = playerClass.skill1;
        skill2 = playerClass.skill2;
    }

    private void Awake()
    {
        // Warrior, Paladin, Wizard, Conduit, Wraith
        // starting stat total = 13
        SetPlayerClass(playerClass);
        current_health = max_health;
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

        rb.velocity = speed * move_coefficient * Time.deltaTime * new Vector2(x_direction, y_direction).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var current_velocity = rb.velocity;
            rb.velocity *= stamina * dodge_coefficient;
        }
    }

    private void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var current_velocity = rb.velocity;
            rb.velocity *= stamina * dodge_coefficient;
        }
    }
}
