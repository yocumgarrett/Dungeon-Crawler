using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPosition : MonoBehaviour
{
    Vector3 mousePos;
    public float radius;
    public GameObject attackPositionObject;

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        Vector3 direction = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0).normalized;

        Vector3 attackPosition = direction * radius;
        attackPositionObject.transform.position = transform.position + attackPosition;

        float directionAngle = CalculateDirectionAngle(direction);
        attackPositionObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, directionAngle));
    }

    private float CalculateDirectionAngle(Vector3 direction)
    {
        return ( Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg ) - 90f;
    }
    
}
