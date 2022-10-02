using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public PowerupEffect powerupEffect;
    private float spawnWaitTime = 1f;
    private Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponent<CircleCollider2D>();
        if (myCollider)
            StartCoroutine(WaitForCollection());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Collector>() != null)
        {
            Destroy(gameObject);
            powerupEffect.Apply(collision.gameObject);
        }
    }

    IEnumerator WaitForCollection()
    {
        myCollider.enabled = false;
        yield return new WaitForSeconds(spawnWaitTime);
        myCollider.enabled = true;
    }
}
