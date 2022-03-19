using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Stats")]
    public int max_health;
    public int current_health;
    public int speed;
    public int stamina;
    public int power;
    public int poise;
    public int guard;

    [Header("Attributes")]
    public string player_class;
    public string skill1;
    public string skill2;
    public string passive;
    public int currency = 0;

    public void SetPlayerClass(string _class, int _max_health, int _speed, int _stamina, int _power, int _poise, int _guard, string _skill1, string _skill2, string _passive)
    {
        player_class = _class;
        max_health = _max_health;
        speed = _speed;
        stamina = _stamina;
        power = _power;
        poise = _poise;
        guard = _guard;
        skill1 = _skill1;
        skill2 = _skill2;
        passive = _passive;
    }

    private void Awake()
    {
        // starting stat total = 13
        //SetPlayerClass("Warrior", 110, 2, 2, 4, 3, 2, "Berserk", "Cleave", "Adrenaline");
        //SetPlayerClass("Paladin", 120, 2, 1, 3, 4, 3, "Blood Covenant", "Shield Bash", "Intervention");
        SetPlayerClass("Wizard", 80, 3, 2, 4, 1, 2, "Arcane Aura", "Blink", "Mirrorlink");
        //SetPlayerClass("Conduit", 90, 4, 3, 2, 2, 2, "Magfield", "Whiplash", "Spark");
        //SetPlayerClass("Wraith", 100, 3, 2, 3, 3, 2, "Shadow Umbra", "Shadowstride", "Festering Cuts");
        current_health = max_health;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        // manipulate transform
    }
}
