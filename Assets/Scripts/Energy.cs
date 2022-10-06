using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Energy : MonoBehaviour, ICollectible
{
    enum EnergyState
    {
        Burst,
        Idle,
        Collection
    }
    private EnergyState state;

    public static event Action OnEnergyCollected;
    public Rigidbody2D rb;
    public Sprite[] energySprites;
    Vector3 targetPosition;
    Vector3 idlePosition;
    public float spawnForceScalar;
    public float burstTimeInSeconds;
    public float moveSpeed = 2f;

    public float idleAmplitude;
    public float idleSpeed;
    private float timeSinceIdle = 0f;
    private float yIdle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetSpriteOnSpawn();
        BurstOnSpawn();
    }

    public void Collect()
    {
        Destroy(gameObject);
        GameManager.Instance.EnergyCollected();
        OnEnergyCollected?.Invoke();
    }

    void Update()
    {
        if(state == EnergyState.Idle)
        {
            timeSinceIdle += Time.deltaTime;
            yIdle = Mathf.Sin(timeSinceIdle * idleSpeed) * idleAmplitude;
        }
    }

    private void FixedUpdate()
    {
        // if in range of player, move towards player until destroyed.
        if (state == EnergyState.Collection)
        {
            Vector2 targetDirection = (targetPosition - transform.position).normalized;
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y) * moveSpeed;
        }
        else if(state == EnergyState.Idle)
        {
            transform.position = new Vector3(idlePosition.x, idlePosition.y + yIdle, idlePosition.z);
        }
    }

    public void SetTarget(Vector3 position)
    {
        targetPosition = position;
        state = EnergyState.Collection;
    }

    // assign sprite from list of sprites that gives it a different color
    private void SetSpriteOnSpawn()
    {
        var numSprites = energySprites.Length;
        int spriteIndex = UnityEngine.Random.Range(0, numSprites - 1);
        gameObject.GetComponent<SpriteRenderer>().sprite = energySprites[spriteIndex];
    }

    private void BurstOnSpawn()
    {
        float spawnDirectionX = 2 * UnityEngine.Random.Range(0f, 1f) - 1;
        float spawnDirectionY = 2 * UnityEngine.Random.Range(0f, 1f) - 1;
        Vector2 spawnBurstForce = new Vector2(spawnDirectionX, spawnDirectionY) * spawnForceScalar;
        rb.AddForce(spawnBurstForce);
        state = EnergyState.Burst;
        StartCoroutine(BurstCoroutine());
    }

    IEnumerator BurstCoroutine()
    {
        yield return new WaitForSeconds(burstTimeInSeconds);
        state = EnergyState.Idle;
        rb.velocity = Vector3.zero;
        idlePosition = transform.position;
    }
}
