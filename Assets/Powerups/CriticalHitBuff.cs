using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Powerups/CriticalHitBuff")]
public class CriticalHitBuff : PowerupEffect
{
    public static event Action OnCriticalHitBuffCollected;

    public float amount;
    public FloatVariable CriticalChance;

    public override void Apply(GameObject target)
    {

        CriticalChance.value = Mathf.Clamp(CriticalChance.value + amount, 0, 1);
        OnCriticalHitBuffCollected?.Invoke();
    }
}
