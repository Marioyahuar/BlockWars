using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitingManager : GenericSingleton<RecruitingManager>
{
    public City CitySelected()
    {
        return PlayerData.Instance.userEmpire.GetCities()[PlayerData.Instance.GetSelectedCity()];
    }
   /* IEnumerator Recruit()
    {
        if (Recruiting)
        {
            if (recruitingTimeLeft > 0)
            {
                recruitingTimeLeft -= Time.deltaTime;
                //UpdateTimer(roundTimeLeft, roundTimerTxt);
            }
            else
            {
                Debug.Log("RecruitComplete");
                Recruiting = false;
                recruitingTimeLeft = recruitingTime;
            }
        }
        yield return new WaitForFixedUpdate();
    }*/
}
