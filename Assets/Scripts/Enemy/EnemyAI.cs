using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    bool has_target;
    bool chase;
    public string target_tag = "Player";
    public float speed = 1f;
    public void SetChase(bool _chase) { chase = _chase; }
    public Rigidbody2D rb;

    public SpriteRenderer spriteRenderer;
    public Sprite[] idleSheet;

    void Start()
    {
        //change has_target to depend on taking damage or seeing player
        rb = GetComponent<Rigidbody2D>();
        has_target = true;
        chase = true;
    }

    void Update()
    {
        /*
        if (has_target && chase)
            ChasePlayer();
        else
            Idle();
            */
    }

    private void FixedUpdate()
    {
        if (has_target && chase)
            ChasePlayer();
        else
            Idle();
    }

    private void ChasePlayer()
    {
        GameObject target = GameObject.FindGameObjectWithTag(target_tag);
        if (target)
        {
            MoveTowardsTargetV2(target.transform.position);
        }
    }

    private void Idle()
    {

    }

    public void MoveTowardsTarget(Vector2 targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        float horizontalDirection = targetPosition.x - transform.position.x;
        float verticalDirection = targetPosition.y - transform.position.y;
        SelectSprite(horizontalDirection, verticalDirection);
    }

    public void MoveTowardsTargetV2(Vector2 targetPosition)
    {
        var direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);

        if (rb)
            rb.velocity = direction * speed * Time.deltaTime;

        SelectSprite(direction.x, direction.y);
    }

    private void SelectSprite(float hDir, float vDir)
    {
        /*
        if (horizontalDirection > 0)
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalDirection < 0)
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        */

        var dir = new Vector2(hDir, vDir);
        var angle = CalculateDirectionAngle(dir);

        // 45 degree arcs +/- 22.5
        if (angle >= 247.5 && angle < 292.5)
            //270
            spriteRenderer.sprite = idleSheet[0];
        else if (angle >= 292.5 && angle < 337.5)
            //315
            spriteRenderer.sprite = idleSheet[1];
        else if (angle >= 337.5 || angle < 22.5)
            //360 / 0
            spriteRenderer.sprite = idleSheet[2];
        else if (angle >= 22.5 && angle < 67.5)
            //45
            spriteRenderer.sprite = idleSheet[3];
        else if (angle >= 67.5 && angle < 112.5)
            //90
            spriteRenderer.sprite = idleSheet[4];
        else if (angle >= 112.5 && angle < 157.5)
            //135
            spriteRenderer.sprite = idleSheet[5];
        else if (angle >= 157.5 && angle < 202.5)
            //180
            spriteRenderer.sprite = idleSheet[6];
        else if (angle >= 202.5 && angle < 247.5)
            //225
            spriteRenderer.sprite = idleSheet[7];
    }

    private float CalculateDirectionAngle(Vector3 direction)
    {
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        return angle;
    }
}
