using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public int damage;

    public void ShootProjectile(Vector3 direction)
    {
        OrientSprite(direction.x);
        rb.AddForce(direction * speed);
    }

    public void OrientSprite(float x)
    {
        var temp_scale = transform.localScale;
        if (x <= 0)
            transform.localScale = new Vector3(Mathf.Abs(temp_scale.x), temp_scale.y, temp_scale.z);
        else if (x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(temp_scale.x), temp_scale.y, temp_scale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage, new Vector2(0, 0));
            Destroy(gameObject);
        }
    }
}
