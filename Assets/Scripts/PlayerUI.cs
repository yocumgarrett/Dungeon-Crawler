using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    public PlayerClass playerClass;
    public TextMeshProUGUI CurrentStatsText;

    void Start()
    {
        SetStatsText();
    }

    void Update()
    {

    }

    public void SetStatsText()
    {
        CurrentStatsText.text = "health " + playerClass.health +
                                "\nspeed " + playerClass.speed +
                                "\nstamina " + playerClass.stamina +
                                "\npower " + playerClass.power +
                                "\npoise " + playerClass.poise +
                                "\nguard " + playerClass.guard;
    }
}
