using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using System.Threading.Tasks;
public class WorldManager : GenericSingleton<WorldManager>
{
    public TileData selectedTile;

    public int ServerRonda = 0;

    public int seed = 4367;

    public Config config = Config.Default();

    private Dictionary<Vector2Int, TileData> _tilesData;

    public Dictionary<Vector2Int, TileData> TilesData { get => _tilesData; }
    public ServerInfo ultimaConsultaServerInfo;
    public DateTime initServerTime;
    public DateTime lastqueryTime;
    public DateTime currentTime;
    public DateTime nextRoundTime;
    /* public void GenerateWorld()
     {
         _tilesData = new Dictionary<Vector2Int, TileData>();
         int countT = 0;
         int countWater = config.waterQty;
         int countBarbarians = config.barbarianQty;
         int count4 = 0;
         int count5 = 0;
         int count6 = 0;
         int count7 = 0;

         for (int i = 0; i < config.width; i++)
         {
             for (int j = 0; j < config.height; j++)
             {
                 CalculatedTile tiledat = CalculateTileRarities();

                 if (tiledat.rarity == 4) count4++;
                 if (tiledat.rarity == 5) count5++;
                 if (tiledat.rarity == 6) count6++;
                 if (tiledat.rarity == 7) count7++;
                 _tilesData[new Vector2Int(i, j)] = new TileData(tiledat.state, tiledat.rarity, User.Empty());
                 countT++;
             }
         }

         while (countWater > -1)
         {
             int x = UnityEngine.Random.Range(1, config.width - 1);
             int y = UnityEngine.Random.Range(1, config.width - 1);
             Vector2Int randpos = new Vector2Int(x, y);
             if (_tilesData[randpos].maxLevel != 7 || _tilesData[randpos].state != TypeSpot.mountain || _tilesData[randpos].state != TypeSpot.barbarians)
             {
                 _tilesData[randpos] = TileData.Mountain();
                 countWater--;
             }
         }

         while (countBarbarians > -1)
         {
             int x = UnityEngine.Random.Range(1, config.width - 1);
             int y = UnityEngine.Random.Range(1, config.width - 1);
             Vector2Int randpos = new Vector2Int(x, y);
             if (_tilesData[randpos].maxLevel != 7 || _tilesData[randpos].state != TypeSpot.mountain || _tilesData[randpos].state != TypeSpot.barbarians)
             {
                 _tilesData[randpos] = TileData.Barbarians();
                 countBarbarians--;
             }
         }

         for (int i = 0; i < config.waterQty; i++)
         {
             int y = UnityEngine.Random.Range(0, config.height);
             int x = UnityEngine.Random.Range(0, config.width);

         }

         //Debug.Log("unclaimed [4]: " + percentage(countT, count4) + "%  " + count4);
         //Debug.Log("unclaimed [5]: " + percentage(countT, count5) + "%  " + count5);
         //Debug.Log("unclaimed [6]: " + percentage(countT, count6) + "%  " + count6);
         //Debug.Log("unclaimed [7]: " + percentage(countT, count7) + "%  " + count7);
     }*/

    private void Awake()
    {
        Application.runInBackground = true;
        //UnityEngine.Random.InitState(seed);
        //CreateNewWorld();
        //GenerateWorld();
    }

    private async void Start()
    {
        ultimaConsultaServerInfo = await Web.ObtenerServerInfo(); //
        initServerTime = DateTime.Parse(ultimaConsultaServerInfo.TiempoInicializado);
        lastqueryTime = currentTime = DateTime.Parse(await Web.ObtenerHoraServer());
        nextRoundTime = CalcularNextRoundTime(lastqueryTime);
        StartCoroutine(RunTime());
    }

    private DateTime CalcularNextRoundTime(DateTime lastTime)
    {
        long timestamp = ConvertToTimestamp(lastTime);
        int duracionRondaenSeg = (ultimaConsultaServerInfo.Duracion * 60);
        long resto = timestamp % duracionRondaenSeg;
        long timestampSigRonda = (timestamp - resto + duracionRondaenSeg);
        var next = ConvertFromTimestamp(timestampSigRonda);
        //Debug.Log("next round time = " + next);
        return next;
    }

    IEnumerator RunTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            //currentTime = currentTime.AddSeconds(1);
            currentTime = DateTime.UtcNow;
            var roundnTime = ShowCurrentRound();
            string txt = string.Format("Round: {0} \n {1:00}:{2:00} Left", roundnTime.round, roundnTime.left.Minutes, roundnTime.left.Seconds);
            UIManager.Instance.SetTextRondaActual(txt);
            if (currentTime >= nextRoundTime.AddSeconds(2))
            {
                //ShowCurrentRound();
                nextRoundTime = CalcularNextRoundTime(currentTime);
                //DatetimeToText(nextRound);
                //ejecutar comando de actualizacion;
                if (PlayerDataSimple.Instance.userWallet != "" && !(PlayerDataSimple.Instance.userID <= 0))
                {
                    Web.Instance.Conectar(PlayerDataSimple.Instance.userWallet);
                }

            }
        }
    }

    public struct RoundnTime
    {
        public int round;
        public TimeSpan left;

        public RoundnTime(int round, TimeSpan left)
        {
            this.round = round;
            this.left = left;
        }
    }

    public RoundnTime ShowCurrentRound()//jalar esta funcion para obtener 
    {
        int lastserverRound = ultimaConsultaServerInfo.Ronda;

        var redondeo = ConvertToTimestamp(CalcularNextRoundTime(lastqueryTime));
        var lastqueryTimeRed = ConvertFromTimestamp(redondeo - (ultimaConsultaServerInfo.Duracion * 60));
        TimeSpan timespan = currentTime - lastqueryTimeRed;
        //Debug.Log("total seconds" + timespan.TotalSeconds);
        //Debug.Log("timespan: " + timespan);
        float r = (float)timespan.TotalSeconds / (ultimaConsultaServerInfo.Duracion * 60);
        //Debug.Log("r" + r);
        int rondaActual = lastserverRound + (int)r;
        ServerRonda = rondaActual;
        //Debug.Log("rondaactual: " + rondaActual);
        var tiempoFaltanteParalaSiguienteRonda = nextRoundTime - currentTime;
        //Debug.Log("segundos" + ((int)tiempoFaltanteParalaSiguienteRonda.TotalSeconds).ToString());
        return new RoundnTime(rondaActual, tiempoFaltanteParalaSiguienteRonda);
    }

    private string DatetimeToText(DateTime time)
    {
        return time.TimeOfDay.ToString();
    }

    public void GenerarEnvioDeTropas(int tipoDeOrden, int tropa) //Funciona para ataques o defensas. tipodeOrden: 3 = Defensa, 4 = Ataque, 5 = colonizacion
    {
        PlayerDataSimple.Instance.CreateOrder(tipoDeOrden, PlayerDataSimple.Instance.GetIDCitySelected(), selectedTile.IDSpot, ServerRonda + 1, tropa, ServerRonda + 1 + CalculateDistance(selectedTile.ubicacion), 0);
    }
    public async void PickTile()
    {
        int distancia = CalculateDistance(selectedTile.ubicacion);
        if (selectedTile.state == TypeSpot.city) { 
            if(selectedTile.uname == "")
            {
                selectedTile.uname = await Web.Instance.ObtenerNombreUsuario(int.Parse(selectedTile.uid));
            }
        }

        UIManager.Instance.RefreshWorldUI(selectedTile, distancia);
        UIManager.Instance.worldUI.OpenOptions();
    }

    public int CalculateDistance(string destino)
    {
        int destinoX = int.Parse(destino.Split(',')[0]);
        int destinoY = int.Parse(destino.Split(',')[1]);
        int inicioX = int.Parse(PlayerDataSimple.Instance.playerVillages[PlayerDataSimple.Instance.GetCitySelected()].villageCoord.Split(',')[0]);
        int inicioY = int.Parse(PlayerDataSimple.Instance.playerVillages[PlayerDataSimple.Instance.GetCitySelected()].villageCoord.Split(',')[1]);
        double distancia = Math.Sqrt(Math.Pow((destinoY - inicioY), 2) + Math.Pow((destinoX - inicioX), 2));
        return (int)distancia;
    }
    public async Task<bool> FetchWorld()
    {
        var array = await Web.Instance.ObtenerMundo();
        _tilesData = Utilities.ConvertArrayToDictionary(array);
        WorldRenderer.Instance.RenderWorld(_tilesData);
        ServerRonda = await Web.Instance.ObtenerRonda();
        //Debug.Log("FetchWorld complete");
        return true;
    }

    public void FetchWorldData()
    {
       // Web.Instance.O
    }

    public Vector2Int GetMapSize()
    {
        return new Vector2Int(config.width, config.height);
    }

    private float percentage(int total, int fraction)
    {
        return ((float)(fraction * 100 / total));
    }


    public TileData GetTileAtPosition(Vector2Int pos)
    {
        //Debug.Log("pos: " + pos);
        if (_tilesData.TryGetValue(pos, out var tile)) return tile;
        return TileData.Null();
    }

    public TileData GetTileFromIDSpot(int idSpot)
    {
        var match = _tilesData.Where(td => td.Value.IDSpot == idSpot);
        return match.FirstOrDefault().Value;
        //return TileData.Null();
    }

    public Tile GetTileSpotFromIDSpot(int idSpot)
    {
        return WorldRenderer.Instance.allTiles.Find(tile => tile.idSpot == idSpot);
    }

    public CalculatedTile CalculateTileRarities()
    {
        float rand = UnityEngine.Random.Range(0.0f, 10.0f);

        if (rand < 3.5) return new CalculatedTile(TypeSpot.valley, 4);
        if (rand < 6.0) return new CalculatedTile(TypeSpot.valley, 5);
        if (rand < 8.0) return new CalculatedTile(TypeSpot.valley, 6);
        if (rand < 10.0) return new CalculatedTile(TypeSpot.valley, 7);

        return new CalculatedTile(TypeSpot.mountain, 0);
    }

    public struct CalculatedTile
    {
        public TypeSpot state;
        public int rarity;

        public CalculatedTile(TypeSpot state, int rarity)
        {
            this.state = state;
            this.rarity = rarity;
        }
    }

    [Serializable]
    public struct Config
    {
        public int width;
        public int height;
        public int waterQty;
        public int barbarianQty;
        public int[] minPercentageRarities;


        public Config(int width, int height, int waterQty, int barbarians, int[] minPercentageRarities)
        {
            this.width = width;
            this.height = height;
            this.waterQty = waterQty;
            this.barbarianQty = barbarians;
            this.minPercentageRarities = minPercentageRarities;
        }

        public static Config Default()
        {
            return new Config(20,20,10,25, new int[4] { 35, 25, 20, 10 });
        }
        public int max()
        {
            return width * height;
        }
    }

    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long ConvertToTimestamp(DateTime value)
    {
        TimeSpan elapsedTime = value - Epoch;
        return (long)elapsedTime.TotalSeconds;
    }
    public static DateTime ConvertFromTimestamp(long timestamp)
    {
        return Epoch.AddSeconds(timestamp);
    }
}

public enum TypeSpot
{
    mountain,
    city,
    valley,
    barbarians,
    owned,
}