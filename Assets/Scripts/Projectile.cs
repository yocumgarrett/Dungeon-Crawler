using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public float rotationSpeed;
    public float damage;
    public int hitCounter;
    public bool directional;
    private int orientation;
    public int damageLayer;

    private void Update()
    {
        transform.Rotate(0, 0, orientation *rotationSpeed);
    }

    public void ShootProjectile(Vector3 direction)
    {
        OrientSprite(direction);
        rb.AddForce(direction.normalized * moveSpeed);
    }

    public void OrientSprite(Vector3 direction)
    {
        if (directional)
        {
            var angle = CalculateDirectionAngle(direction);
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
        else if (!directional)
        {
            if (direction.x <= 0)
                orientation = 1;
            else if (direction.x > 0)
                orientation = -1;
            var temp_scale = transform.localScale;
            transform.localScale = new Vector3(orientation * Mathf.Abs(temp_scale.x), temp_scale.y, temp_scale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == damageLayer)
        {
            var isEnemy = collision.gameObject.GetComponent<Enemy_v1>();
            if (isEnemy)
            {
                isEnemy.TakeDamage(damage, new Vector2(0, 0), null);
                --hitCounter;
                if (hitCounter <= 0)
                {
                    //intead of destroy, make it bounce off of something
                    Destroy(gameObject);
                }
            }
            Debug.Log(collision.transform.GetComponentInChildren<Player_v1>());
            var isPlayer = collision.gameObject.GetComponentInChildren<Player_v1>();
            if (isPlayer)
            {
                
                isPlayer.TakeDamage(damage);
                --hitCounter;
                if(hitCounter <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private float CalculateDirectionAngle(Vector3 direction)
    {
        return (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90f;
    }

}
