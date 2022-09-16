﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendOrderUI : MonoBehaviour
{
    public InputField inputTropas;
    public Text tropaDisponible;
    public Text rondaInit;
    public Text rondaFinish;
    public Button actionBTN;
    public int tipoDeOrden;
    public void RefreshSendOrderUI(int serverRonda, int distancia, int tipoOrden)
    {
        int typeOrder = tipoOrden - 1;
        TipoOrden torden = (TipoOrden)typeOrder;
        tropaDisponible.text = PlayerDataSimple.Instance.GetVillageSelected().troopQty.ToString();
        rondaInit.text = (serverRonda + 1).ToString();
        rondaFinish.text = (serverRonda + 1 + distancia).ToString();
        actionBTN.GetComponentInChildren<Text>().text = torden.ToString();
        tipoDeOrden = tipoOrden;
    }

    public enum TipoOrden
    {
        CrearTropa,
        UpgradeCity,
        Enviar,
        Atacar,
        Colonizar
    }
}
