using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerClass playerClass;
    public TextMeshProUGUI CurrentStatsText;

    public GameObject[] DirectionalUI;
    public GameObject AttackUI;
    public GameObject DashUI;
    public Color defaultColor;
    public Color pressedColor;

    void Start()
    {
        SetStatsText();
    }

    void Update()
    {
        //use an event to update stats text when a stat is changed
        // up
        if(Input.GetAxisRaw("Vertical") > 0)
            DirectionalUI[0].GetComponent<Image>().color = pressedColor;
        else
            DirectionalUI[0].GetComponent<Image>().color = defaultColor;
        // down
        if (Input.GetAxisRaw("Vertical") < 0)
            DirectionalUI[1].GetComponent<Image>().color = pressedColor;
        else
            DirectionalUI[1].GetComponent<Image>().color = defaultColor;
        // left
        if (Input.GetAxisRaw("Horizontal") < 0)
            DirectionalUI[2].GetComponent<Image>().color = pressedColor;
        else
            DirectionalUI[2].GetComponent<Image>().color = defaultColor;
        // right
        if (Input.GetAxisRaw("Horizontal") > 0)
            DirectionalUI[3].GetComponent<Image>().color = pressedColor;
        else
            DirectionalUI[3].GetComponent<Image>().color = defaultColor;
        // dash
        if (Input.GetKey(KeyCode.Z) || Input.GetMouseButton(0))
            DashUI.GetComponent<Image>().color = pressedColor;
        else
            DashUI.GetComponent<Image>().color = defaultColor;
        // attack
        if (Input.GetKey(KeyCode.X) || Input.GetMouseButton(1))
            AttackUI.GetComponent<Image>().color = pressedColor;
        else
            AttackUI.GetComponent<Image>().color = defaultColor;
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
