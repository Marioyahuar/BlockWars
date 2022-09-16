using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerVillage 
{
    public int IDCiudad;
    public string villageName;
    public string villageCoord;
    [SerializeField] bool isCapital;
    public int villageMaxLevel;
    public int villageActualLevel;
    public int troopQty;
    public int troopPerMinute;
    //public bool Recruiting = false;
    //public bool Upgrading = false;

    public PlayerVillage(
        int idCiudad,
        string cityName,
        string cityCoord, //change to vector
        int maxLvl,
        int actLvl,
        int troopQty
        )
    {
        this.IDCiudad = idCiudad;
        this.villageName = cityName;
        this.villageCoord = cityCoord;
        this.villageMaxLevel = maxLvl;
        this.villageActualLevel = actLvl;
        this.troopQty = troopQty;
    }
}
