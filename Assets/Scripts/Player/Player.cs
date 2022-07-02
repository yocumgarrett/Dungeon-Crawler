using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    public static event Action OnPlayerHit;

    [Header("Stats")]
    public FloatVariable MaxHealth, Health;

    [Header("Attributes")]
    public PlayerClass playerClass;

    private void Awake()
    {
        MaxHealth.value = playerClass.health;
        Health.value = playerClass.health;
    }

    private void TakeDamage(float damage)
    {
        Health.value -= damage;
        if (Health.value <= 0)
        {
            Destroy(gameObject);
        }
        OnPlayerHit?.Invoke();
    }

    // whenever the player collides with another collider, unity will call this function
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            TakeDamage(20);
        }
    }
}
