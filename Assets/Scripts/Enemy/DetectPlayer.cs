using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private bool playerDetected;
    private GameObject chaseTarget;
    public GameObject detector;

    private void Start()
    {
        playerDetected = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDetected = true;
            chaseTarget = collision.gameObject;
            var detector_body = detector.GetComponent<Enemy_v1>();
            if (detector_body)
            {
                detector_body.SetState(Enemy_v1.EnemyState.Aggro);
                detector_body.SetTarget(collision.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        var detector_body = detector.GetComponent<Enemy_v1>();
        if (detector_body)
        {
            if(detector_body.GetState() == Enemy_v1.EnemyState.Idle && collision.gameObject.tag == "Player")
            {
                playerDetected = true;
                chaseTarget = collision.gameObject;

                detector_body.SetState(Enemy_v1.EnemyState.Aggro);
                detector_body.SetTarget(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerDetected = false;
            chaseTarget = null;
            var detector_body = detector.GetComponent<Enemy_v1>();
            if (detector_body)
            {
                detector_body.SetState(Enemy_v1.EnemyState.Idle);
                detector_body.SetTarget(null);
                detector_body.myRigidbody.velocity = Vector2.zero;
            }
        }
    }
}
