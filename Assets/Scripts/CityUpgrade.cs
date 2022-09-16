using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CityUpgrade 
{
    [SerializeField] string userID;
    [SerializeField] int cityID;
    [SerializeField] int newLvl;

    public CityUpgrade(string uid,int cityid,int newlvl)
    {
        this.userID = uid;
        this.cityID = cityid;
        this.newLvl = newlvl;
    }
}
