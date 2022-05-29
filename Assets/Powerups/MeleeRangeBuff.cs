using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Powerups/MeleeRangeBuff")]
public class MeleeRangeBuff : PowerupEffect
{
    public static event Action OnAttackRangeBuffCollected;

    public float amount;
    public FloatVariable PlayerMeleeRange;

    public override void Apply(GameObject target)
    {
        PlayerMeleeRange.value += amount;
        OnAttackRangeBuffCollected?.Invoke();
    }

}
