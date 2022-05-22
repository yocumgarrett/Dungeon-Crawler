using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThinker : MonoBehaviour
{
    public Brain brain;

    void Update()
    {
        brain.Think(this);
    }
}
