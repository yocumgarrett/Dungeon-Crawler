using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public int numberOfHits;

    public void TakeHit()
    {
        --numberOfHits;
        if (numberOfHits <= 0)
            Destroy(gameObject);
    }
}
