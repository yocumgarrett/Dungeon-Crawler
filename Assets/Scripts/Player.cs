using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(20);
        }
    }
}
