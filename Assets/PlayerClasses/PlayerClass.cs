using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerClass : ScriptableObject
{
    public string name;
    public float health;
    public float speed;
    public float stamina;
    public float power;
    public float poise;
    public float guard;

    public string passive;
    public string skill1;
    public string skill2;

    public int glort_coin;
}
