using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    private Vector2 offsetVector;
    private Vector2 zeroVector = new Vector2(0, 0);
    Vector3 scaleVector;
    Vector2 spawnPosition;
    public Vector2 GetSpawnPosition() { return spawnPosition; }

    private void Awake()
    {
        scaleVector = gameObject.transform.localScale;
        spawnPosition = gameObject.transform.position;
    }

    public void MoveTowardsTarget(Vector2 targetPosition, Vector2 offsetVector)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        rb.velocity = offsetVector;

        float horizontalDirection = transform.position.x - targetPosition.x;
        FlipSprite(horizontalDirection);
    }

    private void FlipSprite(float horizontalDirection)
    {
        if (horizontalDirection < 0)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(scaleVector.x), scaleVector.y, scaleVector.z);
        }
        else if (horizontalDirection > 0)
        {
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(scaleVector.x), scaleVector.y, scaleVector.z);
        }
    }
}
