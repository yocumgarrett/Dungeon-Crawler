using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/Wander")]
public class WanderBrain : Brain
{
    private float timeBetweenUpdate = 0f;
    public float startTimeBetweenUpdate;
    public float distance;

    private Vector2 movePosition;

    public override void Think(EnemyThinker thinker)
    {
        if(timeBetweenUpdate <= 0)
        {
            Vector2 currentPosition = new Vector2(thinker.transform.position.x, thinker.transform.position.y);
            //make a new random movement choice;
            Vector2 randomDirection = new Vector2(2 * Random.value - 1, 2 * Random.value - 1);
            randomDirection = distance * randomDirection.normalized;
            movePosition = currentPosition + randomDirection;
            
            timeBetweenUpdate = startTimeBetweenUpdate;
        }
        else
        {
            timeBetweenUpdate -= Time.deltaTime;
        }

        var movement = thinker.gameObject.GetComponent<Movement>();
        if (movement)
        {
            movement.MoveTowardsTarget(movePosition);
        }
    }
}
