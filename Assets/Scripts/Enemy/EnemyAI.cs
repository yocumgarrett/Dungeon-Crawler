using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    bool has_target;
    public string target_tag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        has_target = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (has_target)
        {
            ChasePlayer();
        }
        else
        {
            Idle();
        }
    }

    private void ChasePlayer()
    {
        GameObject target = GameObject.FindGameObjectWithTag(target_tag);
        if (target)
        {
            var movement = gameObject.GetComponent<Movement>();
            if (movement)
            {
                movement.MoveTowardsTarget(target.transform.position, new Vector2(0, 0));
            }
        }
    }

    private void Idle()
    {

    }
}
