using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayer : MonoBehaviour
{
    public FloatVariable PlayerHealth;
    public float amount;

    private void OnCollisionEnter(Collision collision)
    {
        Player other = collision.gameObject.GetComponent<Player>();
        if (other)
        {
            Debug.Log("hit player");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("deal damage");
        }
    }
}
