using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_v1 : MonoBehaviour
{
    public Rigidbody2D rb;
    public PlayerClass playerClass;
    public float speedScalar;

    private Vector2 moveDirection;
    Vector3 mousePos;
    Vector3 scaleVector;

    public SpriteRenderer spriteRenderer;
    private Vector2 spriteDirection;
    public Sprite[] idleSheet;

    private void Awake()
    {
        scaleVector = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Process Inputs
        ProcessInputs();

        SelectSprite();
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

        //moveX and moveY are in set [-1, 0, 1].
        // set up switch statement for 8 directional vectors: [-1, -1], [-1, 0], ..., [1, 1]
        spriteDirection = new Vector2(moveX, moveY);
        moveDirection = spriteDirection.normalized;
    }

    private void Move()
    {
        rb.velocity = speedScalar * playerClass.speed * new Vector2(moveDirection.x, moveDirection.y);
    }

    private void SelectSprite()
    {
        if (spriteDirection.x == 0 && spriteDirection.y == -1)
            //sprite 1
            spriteRenderer.sprite = idleSheet[0];
        else if (spriteDirection.x == 1 && spriteDirection.y == -1)
            //sprite 2
            spriteRenderer.sprite = idleSheet[1];
        else if (spriteDirection.x == 1 && spriteDirection.y == 0)
            //sprite 3
            spriteRenderer.sprite = idleSheet[2];
        else if (spriteDirection.x == 1 && spriteDirection.y == 1)
            //sprite 4
            spriteRenderer.sprite = idleSheet[3];
        else if (spriteDirection.x == 0 && spriteDirection.y == 1)
            //sprite 5
            spriteRenderer.sprite = idleSheet[4];
        else if (spriteDirection.x == -1 && spriteDirection.y == 1)
            //sprite 6
            spriteRenderer.sprite = idleSheet[5];
        else if (spriteDirection.x == -1 && spriteDirection.y == 0)
            //sprite 7
            spriteRenderer.sprite = idleSheet[6];
        else if (spriteDirection.x == -1 && spriteDirection.y == -1)
            //sprite 8
            spriteRenderer.sprite = idleSheet[7];

        /*
         * mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = (mousePos - transform.position).normalized;
        float directionAngle = CalculateDirectionAngle(direction);
        Debug.Log(directionAngle);
        var horizontalDirection = direction.x;

        if (horizontalDirection >= 0)
            gameObject.transform.localScale = new Vector3(Mathf.Abs(scaleVector.x), scaleVector.y, scaleVector.z);
        
        else if(horizontalDirection <= 0)
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(scaleVector.x), scaleVector.y, scaleVector.z);
            */
    }

    private float CalculateDirectionAngle(Vector3 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }
}
