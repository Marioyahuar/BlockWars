using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TroopCreation 
{
    [SerializeField] string userID;
    [SerializeField] int cityID;
    [SerializeField] int troopToAdd;
    
    public TroopCreation (string userid, int cityid, int trooptoadd)
    {
        this.userID = userid;
        this.cityID = cityid;
        this.troopToAdd = trooptoadd;
    }
}
