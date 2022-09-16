using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TileData
{
    public int IDSpot;
    public TypeSpot state;
    public int maxLevel;
    public int level;
    public string uid;
    public string uname;
    public string nombre;
    public string ubicacion;
    public int tropas;
    /*
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
     */

    /*Scripts para generación de mundo
    public TileData(TypeSpot state, int maxLevel, User user)
    {
        this.state = state;
        this.maxLevel = maxLevel;
        this.user = user;
        this.nombre = state.ToString();
    }
    public static TileData Valley(int rarity)
    {
        return new TileData(TypeSpot.valley, rarity, User.Empty());
    }
    public static TileData Mountain()
    {
        return new TileData(TypeSpot.mountain, 0, User.Empty());
    }

    public static TileData Barbarians(){
        return new TileData(TypeSpot.barbarians, 1, new User("","Barbarian"));
    }

     */

    public TileData(MundoSpot mundoSpot)
    {
        this.IDSpot = mundoSpot.IDMundoSpot;
        this.state = Utilities.StringToTileState(mundoSpot.TipoSpot);
        this.maxLevel = mundoSpot.NivelMax;
        this.level = mundoSpot.NivelActual;
        this.uid = mundoSpot.IDUsuario.ToString();
        this.uname = "";
        this.nombre = mundoSpot.Nombre;
        this.ubicacion = mundoSpot.Ubicacion;
        this.tropas = mundoSpot.Tropas;
    }

    public static TileData Null()
    {
        return new TileData();
    }
}
