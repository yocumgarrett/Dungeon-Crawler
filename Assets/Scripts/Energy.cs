using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Energy : MonoBehaviour, ICollectible
{
    public static event Action OnEnergyCollected;
    Rigidbody2D rb;
    public float spawnForceScalar = 10f;

    bool hasTarget;
    Vector3 targetPosition;
    public float moveSpeed = 2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        BurstOnSpawn();
    }

    public void Collect()
    {
        Destroy(gameObject);
        OnEnergyCollected?.Invoke();
    }

    private void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector2 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * moveSpeed;
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        hasTarget = true;
    }

    private void BurstOnSpawn()
    {
        float spawnDirectionX = 2 * UnityEngine.Random.Range(0f, 1f) - 1;
        float spawnDirectionY = 2 * UnityEngine.Random.Range(0f, 1f) - 1;
        Vector2 spawnBurstForce = new Vector2(spawnDirectionX, spawnDirectionY) * spawnForceScalar;
        rb.AddForce(spawnBurstForce);
    }
}
