using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Overlay : MonoBehaviour
{
    GameObject Player;
    public TextMeshProUGUI CurrentStatsText;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        SetStatsText();
    }

    void Update()
    {
        
    }

    public void SetStatsText()
    {
        CurrentStatsText.text = "health " + Player.GetComponent<Player>().GetHealth() +
                                "\nspeed " + Player.GetComponent<Player>().GetSpeed() +
                                "\nstamina " + Player.GetComponent<Player>().GetStamina() +
                                "\npower " + Player.GetComponent<Player>().GetPower() +
                                "\npoise " + Player.GetComponent<Player>().GetPoise() +
                                "\nguard " + Player.GetComponent<Player>().GetGuard();
    }
}
