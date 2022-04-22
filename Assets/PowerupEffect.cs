using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract means this delegate can never be instantiated
// a template of what we want our scriptable objects to be
public abstract class PowerupEffect : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
