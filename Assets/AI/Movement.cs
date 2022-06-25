using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rb;
    Transform objectTransform;
    public float speed;
    private Vector2 offsetVector;

    public void MoveTowardsTarget(Vector2 targetPosition, float offset = 0f)
    {
        offsetVector = new Vector2(offset, offset);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition + offsetVector, speed * Time.deltaTime);
    }
}
