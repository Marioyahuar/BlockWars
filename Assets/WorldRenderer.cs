using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRenderer : GenericSingleton<WorldRenderer>
{

    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;
    [SerializeField] private WorldHandler worldHandler;
    public List<Tile> allTiles;
    public Tile tilePicked;
    public Sprite myVillage;
    public Sprite hovermyVillage;
    public Sprite enemyVillage;
    public Sprite hoverenemyVillage;
    public Sprite barbarian;
    public Sprite hoverbarbarian;
    public Sprite nonColonizable;
    public Sprite hovernonColonizable;
    public Sprite valley;
    public Sprite hovervalley;
    public Sprite cityAttacked;
    public Sprite cityCreating;
    public Sprite cityUpgrading;
    public Sprite cityColonizando;


    public GameObject Parent;
    void Start()
    {
        CreateHandler();
    }
    void CreateHandler()
    {
        //worldHandler = new WorldHandler();
    }
    public void RenderWorld(Dictionary<Vector2Int,TileData> world)
    {
        //Debug.Log("world.Count: " + world.Count);
        int mapSize =(int)Mathf.Sqrt((float)world.Count);
        //Debug.Log("mapSize: " + mapSize);
        //GameObject Parent = new GameObject("World Tiles");

        foreach(Transform spot in Parent.transform)
        {
            Destroy(spot.gameObject);
        }
        allTiles.Clear();
        string userid = PlayerDataSimple.Instance.userID.ToString();
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                TileData tileData = world[new Vector2Int(x, y)];
                Tile spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, Parent.transform);
                spawnedTile.name = $"Tile [{x}, {y}]";
                spawnedTile.idSpot = tileData.IDSpot;
                //var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                //spawnedTile.Init(isOffset);
                if(tileData.state == TypeSpot.valley)
                {
                    //spawnedTile.Init(WhiteGradient(tileData.maxLevel));
                    spawnedTile.Init(valley,hovervalley);
                }
                else
                {
                    if (tileData.uid == userid)
                    {

                        //spawnedTile.Init(Color.green); //Para usar color
                        spawnedTile.Init(myVillage,hovermyVillage);
                    }
                    else 
                    {
                        //spawnedTile.Init(GetColorState(tileData.state)); //Para usar color
                        spawnedTile.Init(GetSpriteState(tileData.state),GetHoverSpriteState(tileData.state));
                    }

                }
                allTiles.Add(spawnedTile);
            }
        }
        _cam.transform.position = new Vector3((float)mapSize / 2 - 0.5f, (float)mapSize / 2 - 0.5f, -10);
    }
    public Color WhiteGradient(int maxLevel)
    {
        switch (maxLevel)
        {
            case 5:
                return new Color(.85f, .85f, .85f, 1f);
            case 6:
                return new Color(.7f, .7f, .7f, 1f);
            case 7:
                return new Color(.55f, .55f, .55f, 1f);
            default:
                return Color.white;
        }
    }
    public Color GetColorState(TypeSpot tilestate)
    {
        switch (tilestate)
        {
            case TypeSpot.city:
                return Color.cyan;
            case TypeSpot.barbarians:
                return Color.magenta;
            case TypeSpot.valley:
                return Color.white;
            case TypeSpot.mountain:
                return Color.blue;
            case TypeSpot.owned:
                return Color.green;
            default:
                return Color.black;
        }
    }

    public Sprite GetSpriteState(TypeSpot tilestate)
    {
        switch (tilestate)
        {
            case TypeSpot.city:
                return hoverenemyVillage;
            case TypeSpot.barbarians:
                return hoverbarbarian;
            case TypeSpot.valley:
                return hovervalley;
            case TypeSpot.mountain:
                return hovernonColonizable;
            case TypeSpot.owned:
                return hovermyVillage;
            default:
                return null;
        }
    }

    public Sprite GetHoverSpriteState(TypeSpot tilestate)
    {
        switch (tilestate)
        {
            case TypeSpot.city:
                return enemyVillage;
            case TypeSpot.barbarians:
                return barbarian;
            case TypeSpot.valley:
                return valley;
            case TypeSpot.mountain:
                return nonColonizable;
            case TypeSpot.owned:
                return myVillage;
            default:
                return null;
        }
    }

    /*public Tile GetTileAtPosition(Vector2 pos)
     {
         if (_tiles.TryGetValue(pos, out var tile)) return tile;
         return null;
     }*/

}
