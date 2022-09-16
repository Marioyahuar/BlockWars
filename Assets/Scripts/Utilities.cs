using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static Dictionary<Vector2Int, TileData> ConvertArrayToDictionary(MundoSpot[] spots)
    {
        Dictionary<Vector2Int, TileData> tiledatas = new Dictionary<Vector2Int, TileData>();
        for (int i = 0; i < spots.Length; i++)
        {
            tiledatas[convertStringToVector2(spots[i].Ubicacion)] = new TileData(spots[i]);
        }
        return tiledatas;
    }

    public static Vector2Int convertStringToVector2(string coord)
    {
        string[] splited = coord.Split(',');
        int x = int.Parse(splited[0]);
        int y = int.Parse(splited[1]);
        return new Vector2Int(x, y);
    }

    public static string GetCoord2String(Vector3Int cityCoord)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendFormat("[{0},{1}]", cityCoord.x, cityCoord.y);
        return sb.ToString();
    }



    public static TypeSpot StringToTileState(string tipoSpot)
    {
        switch (tipoSpot)
        {
            case "Valle": return TypeSpot.valley;
            case "Ciudad": return TypeSpot.city;
            case "Barbaros": return TypeSpot.barbarians;
            default: return TypeSpot.valley;
        }
    }
}
