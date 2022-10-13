using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Powerups/ProjectileBuff")]
public class ProjectileBuff : PowerupEffect
{
    public static event Action OnProjectileBuffCollected;

    public float amount;
    public FloatVariable PlayerMaxProjectiles;
    public FloatVariable PlayerCurrentProjectiles;

    public override void Apply(GameObject target)
    {
        PlayerCurrentProjectiles.value = Mathf.Clamp(PlayerCurrentProjectiles.value + amount, 0, PlayerMaxProjectiles.value);
        GameManager.Instance.NumberProjectilesChanged();
        OnProjectileBuffCollected?.Invoke();
    }
}
