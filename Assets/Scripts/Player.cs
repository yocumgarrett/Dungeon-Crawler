using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float current_health;

    [Header("Attributes")]
    public PlayerClass playerClass;

    [Header("Colors")]
    public Color defaultColor;
    public Color dashingColor;
    public Color attackColor;

    private void Awake()
    {
        current_health = playerClass.health;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

}
