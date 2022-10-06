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
    private int orientation;

    private void Update()
    {
        transform.Rotate(0, 0, orientation *rotationSpeed);
    }

    public void ShootProjectile(Vector3 direction)
    {
        if(direction.x <= 0)
            orientation = 1;
        else if (direction.x > 0)
            orientation = -1;
        OrientSprite(orientation);
        rb.AddForce(direction * moveSpeed);
    }

    public void OrientSprite(int orient)
    {
        var temp_scale = transform.localScale;
        transform.localScale = new Vector3(orient* Mathf.Abs(temp_scale.x), temp_scale.y, temp_scale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy_v1>().TakeDamage(damage, new Vector2(0, 0), null);
            --hitCounter;
            if (hitCounter <= 0)
            {
                //intead of destroy, make it bounce off of something
                Destroy(gameObject);
            }
        }
    }

}
