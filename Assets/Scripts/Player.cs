using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public int speed_coefficient;

    [Header("Stats")]
    public int max_health;
    public int current_health;
    public int speed;
    public int stamina;
    public int power;
    public int poise;
    public int guard;

    public int GetHealth() { return max_health; }
    public int GetSpeed() { return speed; }
    public int GetStamina() { return stamina; }
    public int GetPower() { return power; }
    public int GetPoise() { return poise; }
    public int GetGuard() { return guard; }

    [Header("Attributes")]
    public string player_class;
    public string skill1;
    public string skill2;
    public string passive;
    public int currency = 0;

    public void SetPlayerClass(string _class)
    {
        switch (_class)
        {
            case "Warrior":
                player_class = _class;
                max_health = 110;
                speed = 2;
                stamina = 2;
                power = 4;
                poise = 3;
                guard = 2;
                skill1 = "Berserk";
                skill2 = "Cleave";
                passive = "Adrenaline";
                break;
            case "Paladin":
                player_class = _class;
                max_health = 120;
                speed = 2;
                stamina = 1;
                power = 3;
                poise = 4;
                guard = 3;
                skill1 = "Blood Covenant";
                skill2 = "Shield Bash";
                passive = "Intervention";
                break;
            case "Wizard":
                player_class = _class;
                max_health = 80;
                speed = 3;
                stamina = 2;
                power = 4;
                poise = 1;
                guard = 2;
                skill1 = "Arcane Aura";
                skill2 = "Blink";
                passive = "Mirrorlink";
                break;
            case "Conduit":
                player_class = _class;
                max_health = 90;
                speed = 4;
                stamina = 3;
                power = 2;
                poise = 2;
                guard = 2;
                skill1 = "Magfield";
                skill2 = "Whiplash";
                passive = "Spark";
                break;
            case "Wraith":
                player_class = _class;
                max_health = 100;
                speed = 3;
                stamina = 2;
                power = 3;
                poise = 3;
                guard = 2;
                skill1 = "Shadow Umbra";
                skill2 = "Shadowstride";
                passive = "Festering Cuts";
                break;
            default:
                player_class = "Unknown";
                max_health = 100;
                speed = 3;
                stamina = 3;
                power = 3;
                poise = 3;
                guard = 1;
                skill1 = "N/A";
                skill2 = "N/A";
                passive = "N/A";
                break;
        }
    }
    public void ModifyHealth(int amount) { current_health += amount; }
    public void ModifySpeed(int amount) { speed += speed; }
    public void ModifyStamina(int amount) { stamina += amount; }
    public void ModifyPower(int amount) { power += amount; }
    public void ModifyPoise(int amount) { poise += amount; }
    public void ModifyGuard(int amount) { guard += amount; }

    private void Awake()
    {
        // Warrior, Paladin, Wizard, Conduit, Wraith
        // starting stat total = 13
        SetPlayerClass("Warrior");
        current_health = max_health;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // manipulate rigidbody 2D velocity
        var x_velocity = Input.GetAxis("Horizontal") * speed * speed_coefficient * Time.deltaTime;
        var y_velocity = Input.GetAxis("Vertical") * speed * speed_coefficient * Time.deltaTime;

        rb.velocity = new Vector2(x_velocity, y_velocity);
    }
}
