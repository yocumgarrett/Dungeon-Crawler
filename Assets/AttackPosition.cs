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
        //Debug.Log(mousePos);
        
        Vector2 direction = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
        Debug.Log(direction);

        Vector3 attackPosition = direction * radius;
        attackPositionObject.transform.position = transform.position + attackPosition;
        //attackPositionObject.transform.rotation = Quaternion.Euler(attackPosition.x, attackPosition.y, attackPosition.z);
        //attackPositionObject.transform.rotation = Quaternion.Euler(attackPosition.x, attackPosition.y, attackPosition.x + attackPosition.y);
    }
}
