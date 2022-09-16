using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : GenericSingleton<GameManager>
{
    public float roundTime = 60;
    public float calculationTime = 10;
    public float roundTimeLeft;
    public bool roundTimerOn = false;
    public Text roundTimerTxt;
    public float calculationTimeLeft;
    public bool calculationTimerOn = false;
    public Text calculationTimerTxt;

    void Update()
    {
        if (roundTimerOn)
        {
            if(roundTimeLeft > 0)
            {
                roundTimeLeft -= Time.deltaTime;
                UpdateTimer(roundTimeLeft, roundTimerTxt);
            }
            else
            {
                Debug.Log("Time is UP!");
                roundTimeLeft = 0;
                roundTimerOn = false;
                calculationTimerOn = true;
                calculationTimeLeft = calculationTime;
            }
        }

        if (calculationTimerOn)
        {
            if (calculationTimeLeft > 0)
            {
                calculationTimeLeft -= Time.deltaTime;
                UpdateTimer(calculationTimeLeft, calculationTimerTxt);
            }
            else
            {
                Debug.Log("Time is UP!");
                calculationTimeLeft = 0;
                calculationTimerOn = false;
                roundTimerOn = true;
                roundTimeLeft = roundTime;
            }
        }
    }

    void UpdateTimer(float currentTime, Text currentTimer)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        currentTimer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
