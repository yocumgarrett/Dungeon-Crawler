using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    public float amount;
    public FloatVariable PlayerHealth;
    public FloatVariable PlayerMaxHealth;

    public override void Apply(GameObject target)
    {

        PlayerHealth.value = Mathf.Clamp(PlayerHealth.value + amount, 0, PlayerMaxHealth.value);
    }
}
