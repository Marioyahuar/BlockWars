using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : GenericSingleton<UIManager>
{
    public GameObject LoginPanel;
    public GameObject CityPanel;
    public GameObject WorldPanel;
    public GameObject World;
    public GameObject GeneralinfoPanel;
    public Dropdown[] userCitiesDD;
    public InputField wallet;
    public InputField userName;
    public InputField walletToLogin;
    public CityUI cityUI;
    public WorldUI worldUI;
    public SendOrderUI sendOrderUI;
    public GameObject[] panelesBlock;
    public MovimientosUI movimientosUI;
    public ReportesUI reportesUI;
    public ReporteDetail reporteDetail;
    public Text rondaActual;
    public Text globalUserName;
    public GameObject Avisos;

    public GameObject panelCrearUser;
    public GameObject panelIniciarJuego;


    public GameObject initPanelv2;
    public Button btnLogin;
    public Button btnRegistrar;
    public Button btnIniciar;

    public InputField inUserName;
    public Text textWallet;
    public Text textUser;
    public int tropasParaColonizar = 20;
    private void Start()
    {
        textWallet.text = "";
        textUser.text = "";
        panelCrearUser.SetActive(false);
        panelIniciarJuego.SetActive(false);
    }

    public async void OnConnectWallet()
    {
        if (await Web.Instance.OnConnectWalletVerifyIsUserExist())
        {
            btnLogin.interactable = false;
            panelCrearUser.SetActive(false);
            panelIniciarJuego.SetActive(true);
        }
        else
        {
            btnLogin.interactable = false;
            panelCrearUser.SetActive(true);
            panelIniciarJuego.SetActive(false);
            
        }
        
    }

    public void onEdit()
    {
        if(inUserName.text.Length > 4)
        {
            btnRegistrar.interactable = true;
        }
        else
        {
            btnRegistrar.interactable = false;
        }
    }

    public void SetTextRondaActual(string text)
    {
        rondaActual.text = text;
    }

    public void SetAvisos(string aviso)
    {
        Avisos.SetActive(true);
        Avisos.GetComponent<Avisos>().SetAviso(aviso);
    }
    public void RefreshCityUI(int cityIndex)
    {
        cityUI.cityName.text = PlayerDataSimple.Instance.playerVillages[cityIndex].villageName;
        cityUI.cityNivel.text = PlayerDataSimple.Instance.playerVillages[cityIndex].villageActualLevel.ToString();
        cityUI.cityMaxNivel.text = PlayerDataSimple.Instance.playerVillages[cityIndex].villageMaxLevel.ToString();
        cityUI.cityProductionRate.text = cityUI.cityNivel.text + "\n" + "TROOPS";
        cityUI.cityTropaSubirNivel.text = ((int)Mathf.Pow(PlayerDataSimple.Instance.playerVillages[cityIndex].villageActualLevel, 2) * 10).ToString() + "\n" + "TROOPS";
        foreach(Text troopTXT in cityUI.cityTropas) troopTXT.text = PlayerDataSimple.Instance.playerVillages[cityIndex].troopQty.ToString() + "\n" + "TROOPS";

    }
    public async void VerMovimientos()
    {
        PlayerDataSimple.Instance.ordenes = await Web.Instance.ObtenerOrdenes(PlayerDataSimple.Instance.userID);
        movimientosUI.RefreshMovimientosPanel(PlayerDataSimple.Instance.ordenes);
        movimientosUI.gameObject.SetActive(true);
    }
    public async void VerReportes()
    {
        PlayerDataSimple.Instance.reportes = await Web.Instance.GetUserReportes(PlayerDataSimple.Instance.userID);
        reportesUI.RefreshReportesPanel(PlayerDataSimple.Instance.reportes);
        reportesUI.gameObject.SetActive(true);
    }
    public void VerReporteDetail(Reporte report)
    {
        reporteDetail.gameObject.SetActive(true);
        reporteDetail.RefreshReporteDetails(report);
    }
    public void RefreshWorldUI(TileData tile, int distance)
    {
        if (tile.state == TypeSpot.city)
        {
            worldUI.userName.text = tile.uname;
            worldUI.tropa.text = "TROOPS: ¿?";
        }
        else
        {
            worldUI.userName.text = tile.state.ToString();
            worldUI.tropa.text = "TROOPS: " + tile.tropas.ToString();
        }

        worldUI.villageCoord.text = "Coords: " + tile.ubicacion;
        worldUI.nivel.text = "LEVEL: " + tile.level;
        worldUI.nivelMax.text = "LEVEL MAX: " + tile.maxLevel;
        worldUI.distance.text = "DISTANCE: " + distance;

        RefreshButtons(tile);

    }
    public void RefreshButtons(TileData tile)
    {
        if (tile.uid == PlayerDataSimple.Instance.userID.ToString())
        {
            worldUI.AtackBTN.gameObject.SetActive(false);
            worldUI.ConquerBTN.gameObject.SetActive(false);
            worldUI.SendBTN.gameObject.SetActive(true);
        }
        else if(tile.state == TypeSpot.barbarians)
        {
            worldUI.AtackBTN.gameObject.SetActive(true);
            worldUI.ConquerBTN.gameObject.SetActive(false);
            worldUI.SendBTN.gameObject.SetActive(false);
        }
        else if(tile.state == TypeSpot.valley)
        {
            worldUI.AtackBTN.gameObject.SetActive(false);
            worldUI.ConquerBTN.gameObject.SetActive(true);
            worldUI.SendBTN.gameObject.SetActive(false);
        }
        else if(tile.state == TypeSpot.mountain)
        {
            worldUI.AtackBTN.gameObject.SetActive(false);
            worldUI.ConquerBTN.gameObject.SetActive(false);
            worldUI.SendBTN.gameObject.SetActive(false);
        }
        else
        {
            worldUI.AtackBTN.gameObject.SetActive(true);
            worldUI.ConquerBTN.gameObject.SetActive(false);
            worldUI.SendBTN.gameObject.SetActive(true);
        }
    }
    public void ActivateSendOrderPanel(int tipoOrden)
    {
        sendOrderUI.gameObject.SetActive(true);
        sendOrderUI.RefreshSendOrderUI(WorldManager.Instance.ServerRonda, WorldManager.Instance.CalculateDistance(WorldManager.Instance.selectedTile.ubicacion),tipoOrden);
    }
    public void Empezar()
    {
        Web.Instance.Empezar();
    }
    public void CrearOrdenDeEnvioDeTropa()//Funciona para ataques y defensas (y colonizaciones)
    {
        if (sendOrderUI.tipoDeOrden == 5)
        {
            if (PlayerDataSimple.Instance.GetVillageSelected().troopQty >= tropasParaColonizar)
            {
                WorldManager.Instance.GenerarEnvioDeTropas(sendOrderUI.tipoDeOrden, tropasParaColonizar);
            }
            else
            {
                SetAvisos("You don't have enough troops.");
            }
        }
        else
        {
            if (int.Parse(sendOrderUI.inputTropas.text) > PlayerDataSimple.Instance.GetVillageSelected().troopQty)
            {
                //Debug.Log("No tienes tantas tropas");
                SetAvisos("You don't have that many troops.");
                return;
            }
            WorldManager.Instance.GenerarEnvioDeTropas(sendOrderUI.tipoDeOrden, int.Parse(sendOrderUI.inputTropas.text));
        }
        
    }
    public void CrearOrdenSubidaDeNivel()
    {
        int troopToSacrifice = (int)Mathf.Pow(PlayerDataSimple.Instance.GetVillageSelected().villageActualLevel, 2) * 10;
        if (PlayerDataSimple.Instance.GetVillageSelected().troopQty < troopToSacrifice || PlayerDataSimple.Instance.GetVillageSelected().villageActualLevel == PlayerDataSimple.Instance.GetVillageSelected().villageMaxLevel)
        {
            //Debug.Log("No tienes tantas tropas o ya alcanzaste el nivel máximo");
            UIManager.Instance.SetAvisos("You need more troops to level up your village");
            return;
        }
        PlayerDataSimple.Instance.CreateOrder(2, PlayerDataSimple.Instance.GetIDCitySelected(), PlayerDataSimple.Instance.GetIDCitySelected(), WorldManager.Instance.ServerRonda, troopToSacrifice, WorldManager.Instance.ServerRonda + 1, 0); //IMPORTANTE >> El descuento de tropa por subida de nivel debería ser controlado por servidor. El control del nivel también. 
    }
    public void CrearOrdenFabricarTropa()
    {
        PlayerDataSimple.Instance.CreateOrder(1, PlayerDataSimple.Instance.GetIDCitySelected(), PlayerDataSimple.Instance.GetIDCitySelected(), WorldManager.Instance.ServerRonda, PlayerDataSimple.Instance.GetVillageSelected().villageActualLevel, WorldManager.Instance.ServerRonda + 1, 0);
    }
    public void Conectar()
    {
        Web.Instance.Conectar(walletToLogin.text);// GetUserID(walletToLogin.text);
    }

    public void ConectarRegistro()
    {
        Web.Instance.ConectarRegistro();
    }
    public void GoToCity()
    {
        LoginPanel.SetActive(false);
        WorldPanel.SetActive(false);
        World.SetActive(false);
        CityPanel.SetActive(true);
        GeneralinfoPanel.SetActive(true);
    }
    public void GoToWorld()
    {
        LoginPanel.SetActive(false);
        WorldPanel.SetActive(true);
        World.SetActive(true);
        CityPanel.SetActive(true);
        GeneralinfoPanel.SetActive(true);

        panelCrearUser.SetActive(false);
        panelIniciarJuego.SetActive(false);
        initPanelv2.SetActive(false);
    }
    public async void CreateUser()
    {
        if(await Web.Instance.CreateUserV2(PlayerDataSimple.Instance.key.PublicAddress, inUserName.text))
        {
            UIManager.Instance.SetAvisos("Account created. You can start the game.");
            panelCrearUser.SetActive(false);
            panelIniciarJuego.SetActive(true);
        }
        else{
            UIManager.Instance.SetAvisos("Account creation failed. Please retry.");
            panelCrearUser.SetActive(true);
            panelIniciarJuego.SetActive(false);
        }
    }
    public void PasarRonda()
    {
        Web.Instance.CambiarRonda();
    }
    public void SetCitiesDropDown()
    {
        List<string> list = new List<string>();
        foreach (PlayerVillage village in PlayerDataSimple.Instance.playerVillages)
        {
            Debug.Log(village.villageCoord);
            list.Add(village.villageCoord);
        }
        foreach(Dropdown ucitydd in userCitiesDD)
        {
            ucitydd.options.Clear();
            foreach (string option in list)
            {
                ucitydd.options.Add(new Dropdown.OptionData(option));
            }
            ucitydd.value = PlayerDataSimple.Instance.GetCitySelected();
            ucitydd.RefreshShownValue();
        }
    }
    public void SetGlobalUsername()
    {
        globalUserName.text = PlayerDataSimple.Instance.userName;
    }
    public void RefreshCitiesDropdownValues(int value)
    {
        foreach(Dropdown ucitydd in userCitiesDD)
        {
            ucitydd.value = value;
            ucitydd.RefreshShownValue();
        }
    }
    public void SelectCity(int newIndex)
    {
        PlayerDataSimple.Instance.SelectCity(newIndex);
        RefreshCitiesDropdownValues(newIndex);
        RefreshCityUI(PlayerDataSimple.Instance.GetCitySelected());
    }
    public bool blockPanelActive()
    {
        //bool block = true;
        //Debug.Log(panelesBlock[0].activeSelf);
        for (int i = 0; i < panelesBlock.Length; i++)
        {
            if (panelesBlock[i].activeSelf)
            {
                return false;
            }
        }

        return true;
    }
}
