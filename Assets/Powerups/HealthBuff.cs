using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerupEffect
{
    public float amount;

    public override void Apply(GameObject target)
    {
        if(target.GetComponent<Player>() != null)
        {
            float clamp_health = target.GetComponent<Player>().Health.value;
            clamp_health = Mathf.Clamp(clamp_health + amount, 0, target.GetComponent<Player>().MaxHealth.value);
            target.GetComponent<Player>().Health.value = clamp_health;
        }
    }
}
