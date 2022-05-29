using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Powerups/MaxHealthBuff")]
public class MaxHealthBuff : PowerupEffect
{
    public static event Action OnMaxHealthBuffCollected;

    public float amount;
    public FloatVariable PlayerHealth;
    public FloatVariable PlayerMaxHealth;

    public override void Apply(GameObject target)
    {
        PlayerMaxHealth.value += amount;
        PlayerHealth.value = PlayerMaxHealth.value;
        OnMaxHealthBuffCollected?.Invoke();
    }
}
