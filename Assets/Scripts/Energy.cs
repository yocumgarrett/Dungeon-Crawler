using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Energy : MonoBehaviour, ICollectible
{
    public static event Action OnEnergyCollected;

    public void Collect()
    {
        Debug.Log("energy collected");
        Destroy(gameObject);
        OnEnergyCollected?.Invoke();
    }
}
