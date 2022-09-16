using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerDataSimple : GenericSingleton<PlayerDataSimple>
{
    [SerializeField] int indexCitySelected = 0;
    public int userID;
    public string userName;
    public string userWallet;
    public List<PlayerVillage> playerVillages;
    public CityOrders[] ordenes;
    public Reporte[] reportes;
    public Key key;

    public void SelectCity(int index)
    {
        if (index < playerVillages.Count && index >= 0)
        {
            indexCitySelected = index;
        }
        else
        {
            indexCitySelected = 0;
        }
    }

    public int GetCitySelected()
    {
        return indexCitySelected; 
    }

    public int GetIDCitySelected()
    {
        return playerVillages[indexCitySelected].IDCiudad;
    }

    public PlayerVillage GetVillageSelected()
    {
        return playerVillages[indexCitySelected];
    }

    public PlayerVillage GetVillageByIDSpot(int IDSpot)
    {
        foreach (PlayerVillage villa in playerVillages)
        {
            if (villa.IDCiudad == IDSpot){
                return villa;
            }
        }
        return null;
    }
    public void CreateOrder(int IDTipoOrden, int IDOrigen, int IDDestino, int RondaInit, int TropasSalida, int RondaFin, int TropasRetorno)
    {
        Order nuevaOrden = new Order();
        nuevaOrden.IDTipo_Orden = IDTipoOrden;
        nuevaOrden.IDCiudadOrigen = IDOrigen;
        nuevaOrden.IDCiudadDestino = IDDestino;
        nuevaOrden.RondaInicio = RondaInit;
        nuevaOrden.TropasSalida = TropasSalida;
        nuevaOrden.Rondafin = RondaFin;
        nuevaOrden.TropasRetorno = TropasRetorno;

        if(IDTipoOrden == 1)
        {
            Web.Instance.CrearFabricacionTropa(nuevaOrden);
        }
        else if(IDTipoOrden == 2)
        {
            Web.Instance.CrearSubidaDeNivel(nuevaOrden);
        }
        else if(IDTipoOrden == 3 || IDTipoOrden == 4 || IDTipoOrden == 5)
        {
            Web.Instance.CrearEnvioTropa(nuevaOrden);
        }
    }
}

/*
        $nuevaOrden = new stdClass();
        $nuevaOrden->IDTipoOrden = 3;
        $nuevaOrden->IDCiudadOrigen = $orden->IDCiudadDestino;
        $nuevaOrden->IDCiudadDestino = $orden->IDCiudadOrigen;
        $nuevaOrden->RondaInicio = $orden->Rondafin;
        $nuevaOrden->TropasSalida = $atacantes - $atacantesDeath;
        $nuevaOrden->RondaFin = $orden->Rondafin + ($orden->Rondafin - $orden->RondaInicio);
        $nuevaOrden->TropasRetorno = 0;*/