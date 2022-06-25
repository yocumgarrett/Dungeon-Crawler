using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/Idle")]
public class IdleSpawnBrain : Brain
{
    private Vector2 spawnPosition;
    private Vector2 endPoint;
    private Vector2 centerPoint;
    private bool spawnSet = false;

    private float timeBetweenUpdate = 0f;
    public float startTimeWait;
    private float maxTimeNoise = .1f;
    public float maxSpawnRadius;
    private float moveRadius;
    private float curveScalar;
    private float offsetValue;

    private bool target = false;

    private float t = 0f;

    public override void Think(EnemyThinker thinker)
    {
        // obtain spawn position. this will become our center point for idling
        if(spawnSet == false)
        {
            spawnPosition = new Vector2(thinker.transform.position.x, thinker.transform.position.y);
            spawnSet = true;
        }



        if(timeBetweenUpdate <= 0)
        {
            var movement = thinker.gameObject.GetComponent<Movement>();
            if (target == false && movement)
            {
                var startPoint = new Vector2(thinker.transform.position.x, thinker.transform.position.y);
                endPoint = new Vector2(spawnPosition.x + Random.Range(-maxSpawnRadius, maxSpawnRadius), spawnPosition.y + Random.Range(-maxSpawnRadius, maxSpawnRadius));
                moveRadius = Vector2.Distance(startPoint, endPoint) / 2;
                centerPoint = (startPoint + endPoint) / 2;
                curveScalar = Random.Range(moveRadius / 2, moveRadius);
                target = true;
                //Debug.Log(endPoint + " -- " + moveRadius);
            }
            
            // keep moving until distance between object and target is basically 0;
            if(t <= 2 * moveRadius)
            {
                t += Time.deltaTime;

                offsetValue = -Mathf.Pow(t - moveRadius, 2f) + moveRadius;

                
                if (movement)
                {
                    movement.MoveTowardsTarget(endPoint, offsetValue);
                }
            }
            else
            {
                t = 0;
                timeBetweenUpdate = Random.Range(startTimeWait, startTimeWait + maxTimeNoise);
                target = false;
            }

            
        }
        else
            timeBetweenUpdate -= Time.deltaTime;
    }

}
