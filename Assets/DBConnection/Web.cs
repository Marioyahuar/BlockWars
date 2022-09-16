using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
public class Web : GenericSingleton<Web>
{
    // Start is called before the first frame update
    async void Start()
    {
        //PlayerDataSimple.Instance.ordenes = await ObtenerOrdenes(1);
        //StartCoroutine(ObtenerIDUsuario());
        //StartCoroutine(GetFirstCity());
        //StartCoroutine(GetUser());
        //StartCoroutine(CreateUser("ElSolitario", "0xsoli", "SPICY"));
        //StartCoroutine(CreateSpots());
        //StartCoroutine(obtenerMundo());
        await ObtenerServerInfo();
    }

    async public Task<bool> OnloginWallet()
    {
        // get current timestamp
        int timestamp = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // set expiration time
        int expirationTime = timestamp + 60;
        // set message
        string message = expirationTime.ToString();
        // sign message
        string signature = await Web3Wallet.Sign(message);
        // verify account
        string account = await EVM.Verify(message, signature);
        int now = (int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds;
        // validate
        if (account.Length == 42 && expirationTime >= now)
        {
            print("Account: " + account);
            PlayerDataSimple.Instance.key = new Key(account);
            // load next scene
            Debug.Log("logged with metamask");
            return true;
        }
        return false;
    }

    public async void ConectarRegistro()
    {
        if (!await OnloginWallet()) { Debug.LogWarning("no se pudo conectar"); };
    }

    public async void Conectar(string userWallet)
    {
        PlayerDataSimple.Instance.userWallet = userWallet;
        //if (!await OnloginWallet()) { Debug.LogWarning("no se pudo conectar"); };
        //PlayerDataSimple.Instance.userID = await ObtenerIDUsuario(PlayerDataSimple.Instance.key.PublicAddress);
        PlayerDataSimple.Instance.userID = await ObtenerIDUsuario(userWallet);
        PlayerDataSimple.Instance.userName = await ObtenerNombreUsuario(PlayerDataSimple.Instance.userID);
        UIManager.Instance.SetGlobalUsername();
        MundoSpot[] aldeas = await GetUserCities(PlayerDataSimple.Instance.userID);
        PlayerDataSimple.Instance.playerVillages.Clear();
        foreach (MundoSpot village in aldeas)
        {
            PlayerVillage newVillage = new PlayerVillage(village.IDMundoSpot, village.Nombre, village.Ubicacion, village.NivelMax, village.NivelActual, village.Tropas);
            PlayerDataSimple.Instance.playerVillages.Add(newVillage);
        }
        UIManager.Instance.SetCitiesDropDown();
        bool test = await WorldManager.Instance.FetchWorld();
        UIManager.Instance.movimientosUI.CheckAttacks();
        UIManager.Instance.GoToWorld();
        UIManager.Instance.SelectCity(0);
    }
    public async void RefreshUserCities()
    {
        MundoSpot[] aldeas = await GetUserCities(PlayerDataSimple.Instance.userID);
        PlayerDataSimple.Instance.playerVillages.Clear();
        foreach (MundoSpot village in aldeas)
        {
            PlayerVillage newVillage = new PlayerVillage(village.IDMundoSpot, village.Nombre, village.Ubicacion, village.NivelMax, village.NivelActual, village.Tropas);
            PlayerDataSimple.Instance.playerVillages.Add(newVillage);
            UIManager.Instance.SetCitiesDropDown();
            UIManager.Instance.SelectCity(PlayerDataSimple.Instance.GetIDCitySelected());
        }
    }
    public void CrearUsuario(string wallet, string username)
    {
        StartCoroutine(CreateUser(wallet, username));
    }
    public void CambiarRonda()
    {
        StartCoroutine(PasarRonda());
    }
    public void ObtenerUsername(int userid)
    {
        StartCoroutine(ObtenerUserName(userid));
    }
    public void CrearFabricacionTropa(Order newOrder)
    {
        StartCoroutine(CreateFabricacionTropa(newOrder));
    }
    public void CrearSubidaDeNivel(Order newOrder)
    {
        StartCoroutine(CreateOrdenSubidaDeNivel(newOrder));
    }
    public void CrearEnvioTropa(Order newOrder)
    {
        StartCoroutine(CreateOrdenEnvioDeTropa(newOrder));
    }
    IEnumerator GetUser()
    {
        int id = 1;
        using (UnityWebRequest www = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerUsuarioPorID.php?id=" + id))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                byte[] results = www.downloadHandler.data;
                Debug.Log(results);
            }
        }

    }
    IEnumerator GetFirstCity()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerPrimerValle.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                IdMundoSpot idMundoSpot = JsonUtility.FromJson<IdMundoSpot>(www.downloadHandler.text);
                int valleID = idMundoSpot.IDMundoSpot;
            }
        }
    }
    IEnumerator CreateUser(string userWallet, string userName)
    {
        UserData newUser = new UserData();
        newUser.Wallet = userWallet;
        newUser.Username = userName;

        string json = JsonUtility.ToJson(newUser);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest("https://block-chest.com/PixelWars/crearUsuario.php", "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        UIManager.Instance.SetAvisos("Cuenta creada. Por favor ingresa con tu dirección de wallet en el panel LOGIN.");
    }
    IEnumerator CreateSpots()//WorldSlot[] Slots)
    {
        int worldXSize = WorldManager.Instance.config.width;
        int worldYSize = WorldManager.Instance.config.height;

        var tiles = WorldManager.Instance.TilesData;

        for (int i = 0; i < worldXSize; i++)
        {
            for (int j = 0; j < worldYSize; j++)
            {
                var tiledata = tiles[new Vector2Int(i, j)];
                WorldSlot newSpot = new WorldSlot();
                newSpot.NivelMax = tiledata.maxLevel;
                newSpot.Ubicacion = i.ToString() + "," + j.ToString();
                newSpot.TipoSpot = tiledata.state.ToString();
                newSpot.IDMapa = 1;
                string json = JsonUtility.ToJson(newSpot);
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                var request = new UnityWebRequest("" +
                    "https://block-chest.com/PixelWars/crearSlot.php", "POST");
                request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                yield return request.SendWebRequest();
                Debug.Log("Status Code: " + request.responseCode);
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    IEnumerator CreateFabricacionTropa(Order order) //Sirve para creacion de tropas
    {
        string json = JsonUtility.ToJson(order);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        var request = new UnityWebRequest("https://block-chest.com/PixelWars/crearFabricacionTropa.php", "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        RefreshUserCities();
        UIManager.Instance.SetAvisos("Una tropa se unirá a la ciudad en la próxima ronda");
    }
    IEnumerator CreateOrdenEnvioDeTropa(Order order) //Sirve para ataques y defensas
    {
        string json = JsonUtility.ToJson(order);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        var request = new UnityWebRequest("https://block-chest.com/PixelWars/crearEnvioTropa.php", "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        RefreshUserCities();
        UIManager.Instance.SetAvisos("Tus tropas iniciarán una misión en la próxima ronda");
    }
    IEnumerator CreateOrdenSubidaDeNivel(Order order) //Sirve para ataques y defensas
    {
        string json = JsonUtility.ToJson(order);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        var request = new UnityWebRequest("https://block-chest.com/PixelWars/crearSubidaDeNivel.php", "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
        RefreshUserCities();
        UIManager.Instance.SetAvisos("Tu ciudad subirá un nivel en la próxima ronda");
    }
    public static IEnumerator InicializarServer(ServerInfo InicializacionServerInfo)
    {
        string json = JsonUtility.ToJson(InicializacionServerInfo);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        var request = new UnityWebRequest("https://block-chest.com/PixelWars/inicializarServer.php", "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();
        Debug.Log("Status Code: " + request.responseCode);
        Debug.Log(request.downloadHandler.text);
    }
    IEnumerator PasarRonda()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://block-chest.com/PixelWars/ejecutarRonda.php"))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                byte[] results = www.downloadHandler.data;
                Debug.Log(results);
            }
        }
    }
    public async Task<int> ObtenerIDUsuario(string userWallet)
    {
        int userID = 0;
        UnityWebRequest www = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerIDUsuario.php?userWallet=" + userWallet);

        await www.SendWebRequest();
    
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            string result = www.downloadHandler.text;
            userID = int.Parse(result);
            //GetUsuarioCities(PlayerDataSimple.Instance.userID);
            //WorldManager.Instance.FetchWorld();
        }

        return userID;
    }
    public async Task<MundoSpot[]> GetUserCities(int userID)
    {
        MundoSpot[] villages = new MundoSpot[0];

        UnityWebRequest www = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerCiudadesPorUser.php?userID=" + userID);

        await www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string newtext = "{\"Items\":" + www.downloadHandler.text + "}";
            villages = JsonHelper.FromJson<MundoSpot>(newtext);
        }
        return villages;
    }
    public async Task<Reporte[]> GetUserReportes(int userID)
    {
        Reporte[] reportes = new Reporte[0];

        UnityWebRequest www = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerReportesPorUser.php?userID=" + userID);

        await www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string newtext = "{\"Items\":" + www.downloadHandler.text + "}";
            reportes = JsonHelper.FromJson<Reporte>(newtext);
        }
        return reportes;
    }
    public async Task<MundoSpot[]> ObtenerMundo()
    {
        MundoSpot[] spots = new MundoSpot[0];

        UnityWebRequest request = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerMundo.php");

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            string newtext = "{\"Items\":" + request.downloadHandler.text + "}";
            //Debug.Log(newtext);
            spots = JsonHelper.FromJson<MundoSpot>(newtext);

            /*Debug.Log("json lenght: " + spots.Length);
            var mundo = Utilities.ConvertArrayToDictionary(spots);
            Debug.Log("mundo lenght: " + mundo.Count);
            WorldRenderer.Instance.RenderWorld(mundo);*/
        }


        return spots;
    }
    public async Task<string> ObtenerNombreUsuario(int userID)
    {
        string nombreUsuario = "";

        UnityWebRequest request = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerUserName.php?userID=" + userID);

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            nombreUsuario = request.downloadHandler.text;
        }


        return nombreUsuario;
    }

    public async Task<Order> ObtenerOrdenPorID(int orderID)
    {
        Order newOrder = new Order();

        UnityWebRequest request = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerOrdenPorID.php?orderID=" + orderID);

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            string newtext = request.downloadHandler.text;
            Debug.Log(newtext);
            newOrder = JsonUtility.FromJson<Order>(newtext);// JsonHelper.FromJson<Order>(newtext);
            Debug.Log(newOrder);
        }


        return newOrder;
    }
    IEnumerator ObtenerUserName(int userID)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerUserName.php?userID=" + userID))
        {
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string userName = www.downloadHandler.text;
                WorldManager.Instance.selectedTile.uname = userName;
            }
        }
    }
    public async Task<int> ObtenerRonda()
    {
        string ronda = "";

        UnityWebRequest request = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerRonda.php");

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            ronda = request.downloadHandler.text;
        }


        return int.Parse(ronda);
    }
    public static async Task<string> ObtenerHoraServer()
    {
        string hora = "";

        UnityWebRequest request = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerHoraServer.php");

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            hora = request.downloadHandler.text;
        }
        return hora;
    }
    public static async Task<ServerInfo> ObtenerServerInfo()
    {
        ServerInfo serverInfo = new ServerInfo();

        UnityWebRequest request = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerServerTime.php");

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            serverInfo = JsonUtility.FromJson<ServerInfo>(request.downloadHandler.text);
            Debug.Log("server: "+ serverInfo.NombreServer);
        }
        return serverInfo;
    }
    public async Task<CityOrders[]> ObtenerOrdenes(int userID)
    {
        CityOrders[] citiesOrders = new CityOrders[0];

        UnityWebRequest request = UnityWebRequest.Get("https://block-chest.com/PixelWars/obtenerOrdenesPorUsuario.php?userID=" + userID);

        await request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            SimpleJSON.JSONNode globalOrdersjson = SimpleJSON.JSON.Parse(request.downloadHandler.text);
            citiesOrders = new CityOrders[globalOrdersjson.Count];
            //Debug.Log(globalOrdersjson.Count);
            for (int i = 0; i < citiesOrders.Length; i++)
            {
                string ordersJson = "{\"Items\":" + globalOrdersjson[i].ToString() + "}";
                Order[] orders = JsonHelper.FromJson<Order>(ordersJson);
                citiesOrders[i] = new CityOrders(orders);
                //Debug.Log(citiesOrders[i].orders[0].IDCiudadDestino);
                //Debug.Log(citiesOrders[i].orders[0].IDCiudadOrigen);
                //Debug.Log(citiesOrders[i].orders[0].IDOrden_en_Curso);
            }
            /*Debug.Log(json[0]);
            Debug.Log(json[0][0]);
            Debug.Log(json[0][0][0]);
            Debug.Log(json[0][0]["Rondafin"]);
            Debug.Log("---------");*/
        }
        return citiesOrders;
    }

}

[System.Serializable]
public class UserData
{
    public string Username;
    public string Wallet;
}

[System.Serializable]
public class WorldSlot
{
    public string Ubicacion;
    public int NivelMax;
    public string TipoSpot;
    public int IDMapa;
}

[System.Serializable]
public class IdMundoSpot
{
    public int IDMundoSpot;
}

[System.Serializable]
public struct MundoSpot
{
    public int IDMundoSpot;
    public string Nombre;
    public string Ubicacion;
    public int NivelMax;
    public int NivelActual;
    public int Tropas;
    public string TipoSpot;
    public int Estado;
    public int IDMapa;
    public int IDUsuario;

    public MundoSpot(
        int iDMundoSpot,
        string nombre,
        string ubicacion,
        int nivelMax,
        int nivelActual,
        int tropas,
        string tipoSpot,
        int estado,
        int iDMapa,
        int iDUsuario)
    {
        IDMundoSpot = iDMundoSpot;
        Nombre = nombre;
        Ubicacion = ubicacion;
        NivelMax = nivelMax;
        NivelActual = nivelActual;
        Tropas = tropas;
        TipoSpot = tipoSpot;
        Estado = estado;
        IDMapa = iDMapa;
        IDUsuario = iDUsuario;
    }
}

[System.Serializable]
public struct Reporte
{
    public int IDReporte;
    public int IDOrdenEncurso;
    public int Fecha;
    public int Resultado;
    public int Estado;
    public int IDAtacante;
    public int PerdidaDefensa;
    public int PerdidaAtaque;
    public int IDDefensor;
    public int Ronda;
    public int Defensores;

    public Reporte(
        int iDReporte,
        int iDOrden,
        int fecha,
        int resultado,
        int estado,
        int iDAtacante,
        int perdidaDefensa,
        int perdidaAtaque,
        int iDDefensor,
        int ronda,
        int defensores
        )
    {
        IDReporte = iDReporte;
        IDOrdenEncurso = iDOrden;
        Fecha = fecha;
        Resultado = resultado;
        Estado = estado;
        IDAtacante = iDAtacante;
        PerdidaDefensa = perdidaDefensa;
        PerdidaAtaque = perdidaAtaque;
        IDDefensor = iDDefensor;
        Ronda = ronda;
        Defensores = defensores;
    }

}
[System.Serializable]
public class Orden
{
    public int tipoOrden;  //1: Crear Tropa, 2: Subir nivel Ciudad, 3: , 4: , 5:
    public int spotOrigen;
    public int spotDestino;
    public int rondaInicio;
    public int tropasSalida;
    public int tropasRetorno;
}

[System.Serializable]
public struct ServerInfo
{
    public int IDServer;
    public string NombreServer;
    public int Tamano;
    public int Ronda;
    public bool Estado;
    public int Duracion;
    public string TiempoRonda;
    public string TiempoInicializado;

    public ServerInfo(int iDServer, string nombreServer, int tamano, int ronda, bool estado, int duracion, string tiempoRonda, string tiempoInicializado)
    {
        IDServer = iDServer;
        NombreServer = nombreServer;
        Tamano = tamano;
        Ronda = ronda;
        Estado = estado;
        Duracion = duracion;
        TiempoRonda = tiempoRonda;
        TiempoInicializado = tiempoInicializado;
    }
}

