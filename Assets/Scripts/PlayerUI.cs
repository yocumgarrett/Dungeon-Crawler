﻿using System.Collections;
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
        //use an event to update stats text when a stat is changed
    }

    public void SetStatsText()
    {
        CurrentStatsText.text = "speed   "   + playerClass.speed +
                                "\nstamina " + playerClass.stamina +
                                "\npower   " + playerClass.power +
                                "\npoise   " + playerClass.poise +
                                "\nguard   " + playerClass.guard;
    }
}
