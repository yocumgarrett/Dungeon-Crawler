using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brains/Idle")]
public class IdleSpawnBrain : Brain
{
    private Vector2 spawnPosition;
    private bool spawnSet = false;

    private float timeBetweenUpdate = 0f;
    public float startTimeWait;
    public float maxRadius;
    public float max_b;
    private float a;
    private float b;

    //private bool move = false;

    public float speed;


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
            // then perform next move
            //move = true;
            var startPoint = new Vector2(thinker.transform.position.x, thinker.transform.position.y);
            var endPoint = new Vector2(spawnPosition.x + Random.Range(-maxRadius, maxRadius), spawnPosition.y + Random.Range(-maxRadius, maxRadius));
            b = Random.Range(-max_b, max_b);
            var directDistance = Vector2.Distance(startPoint, endPoint);
            a = directDistance / 2;
            //centerPoint

            timeBetweenUpdate = startTimeWait;
        }
        else
            timeBetweenUpdate -= Time.deltaTime;
    }

    // startPoint = currentPosition
    // 1. endPoint = randomly selected point in spawn radius
    // 2. curveScalar = randomly selected, gives how much ellipse path will curve (b) of y = bsint
    // 3. calculate direct distance between start and end
    // 4. a = distance / 2
    // 5. centerPoint = midpoint between start and end
    private void EllipsePath(Vector2 centerPoint, float a, float b, float time, Vector2 endPoint)
    {
        // 6. calculate new position as 
        //    newPosition = centerPoint position + (x = acost, y = bsint)
        // 7. move to this new position until reach endPoint.
        //    if (newPosition == endPoint) or (newPosition - endPoint < some small value)
        //      stop moving, move = false;
    }
}
