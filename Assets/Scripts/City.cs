using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class City //: MonoBehaviour
{
    [SerializeField] string cityName;
    [SerializeField] Vector3Int cityCoord;
    [SerializeField] bool isCapital;
    public int cityMaxLevel;
    public int cityActualLevel;
    public int troopQty;
    public int troopPerMinute;
    public bool Recruiting = false;
    public bool Upgrading = false;
    
    //Devuelve el nombre de la ciudad
    public string GetName()
    {
        return cityName;
    }
    
    //Permite cambiar el nombre de la ciudad
    public void SetName(string name)
    {
        cityName = name;
    }
    
    //Devuelve las coordenadas de la ciudad
    public Vector3Int GetCoord()
    {
        return cityCoord;
    }

    //Permite setear las coordenadas de la ciudad
    public void SetCoord(int[] coords)
    {
        cityCoord.x = coords[0];
        cityCoord.y = coords[1];
        cityCoord.z = coords[2];
    }

    //Devuelve si la ciudad es o no capital
    public bool GetIfIsCapital()
    {
        return isCapital;
    }

    //Setea la ciudad como capital
    public void SetIfIsCapital(bool boolisCapital)
    {
        isCapital = boolisCapital;
    }
    public void SetCityAsCapital()
    {
        isCapital = true;
    }

    //Obtiene las coordenadas en formato string
    public string GetCoord2String()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("[{0},{1}]", cityCoord.x, cityCoord.y);
        return sb.ToString();
    }

    //Genera nuevas tropas
    public void CreateTroop()
    {
        if (Recruiting)
        {
            Debug.Log("No puedes iniciar otra tarea de reclutamiento");
            return;
        }
        PendingManager.Instance.AddPendingTroopCreation(PlayerData.Instance.user.GetUid(), PlayerData.Instance.GetSelectedCity(), troopPerMinute);
        Recruiting = true;
    }
    
    //Upgradear ciudad
    public void UpgradeCity()
    {
        if (Upgrading)
        {
            Debug.Log("No puedes iniciar otra tarea de upgrading");
            return;
        }
        PendingManager.Instance.AddPendingCityUpgrade(PlayerData.Instance.user.GetUid(), PlayerData.Instance.GetSelectedCity(), (cityActualLevel + 1));
        Upgrading = true;
    }

    //Inicializa una ciudad en coordenadas 0,0,0
    public static City NewCityZero()
    {
        return NewCity("polis", new Vector3Int(0, 0, 0),4,1,10);
    }
    public static City NewCity(string name, Vector3Int coord, int maxlvl, int actlvl, int troops )
    {
        return new City(
                name,
                coord,
                maxlvl,
                actlvl,
                troops
                );
    }
    
    public City(
        string cityName,
        Vector3Int cityCoord,
        int maxLvl,
        int actLvl,
        int troopQty
        )
    {
        this.cityName = cityName;
        this.cityCoord = cityCoord;
        this.cityMaxLevel = maxLvl;
        this.cityActualLevel = actLvl;
        this.troopQty = troopQty;
    }
    

  
}
