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
    public Color defaultColor;
    public Color pressedColor;

    void Start()
    {
        SetStatsText();
    }

    void Update()
    {
        //use an event to update stats text when a stat is changed
        if(Input.GetAxisRaw("Vertical") > 0)
        {
            //up
            DirectionalUI[0].GetComponent<Image>().color = pressedColor;
        }
        else
            DirectionalUI[0].GetComponent<Image>().color = defaultColor;
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            //down
            DirectionalUI[1].GetComponent<Image>().color = pressedColor;
        }
        else
            DirectionalUI[1].GetComponent<Image>().color = defaultColor;
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            //left
            DirectionalUI[2].GetComponent<Image>().color = pressedColor;
        }
        else
            DirectionalUI[2].GetComponent<Image>().color = defaultColor;
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            //right
            DirectionalUI[3].GetComponent<Image>().color = pressedColor;
        }
        else
            DirectionalUI[3].GetComponent<Image>().color = defaultColor;
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
