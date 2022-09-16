using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData : GenericSingleton<PlayerData>
{
    [SerializeField] int indexCitySelected = 0;

    public User user;
    public Empire userEmpire;
    [SerializeField] Movements userMovements;

    internal override void Awake()
    {
        base.Awake();
        StartPlayerTest();
        //Leer la data del player
    }

    public Movements GetUserMovements()
    {
        return userMovements;
    }
    public void SelectCity(int index)
    {
        if (index < userEmpire.GetCities().Count && index >= 0)
        {
            indexCitySelected = index;
        }
        else
        {
            indexCitySelected = 0;
        }
    }
    public int GetSelectedCity()
    {
        return indexCitySelected;
    }



    public Vector3 GetCoordCitySelected()
    {
        var city = userEmpire.GetCities()[indexCitySelected];
        return city.GetCoord();
    }
    private void StartPlayerTest()
    {
        //user = new User("5", "Akira1234");
        userEmpire = new Empire(Empire.Starter());
        userEmpire.GetCities()[0].SetCityAsCapital();
    }

    // Eliminar al final
    /*
    public void SavePlayerData()
    {
        SaveSystem.SaveData(this);
    }

    public void LoadPlayerData()
    {
        PlayerSaveFile data = SaveSystem.LoadData();

        user = data.user;
        userGold = data.userGold;
        userMerchants = data.userMerchants;
        userTechs = data.userTechs;
        userEmpire.DeSerializeEmpire(data.userEmpire);

        RefresherManager.Instance.ExecuteCasualRefresher();
    }

    public void SaveCityData()
    {
        SaveSystem.SaveCityData(this.userEmpire.GetCapitalCity());
    }
    */
}

