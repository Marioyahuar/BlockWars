using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovimientosUI : MonoBehaviour
{
    public Transform parent;
    public GameObject movimientoPrefab;
    public GameObject movButton;
    string[] tipoMovs = { "Reclutar", "Ampliar", "Enviar", "Atacar", "Colonizar" };
    public Sprite[] movsSprites;
    public Sprite ataqueentrante;

    public async void RefreshMovimientosPanel(CityOrders[] ordenes)
    {
        List<int> movids = new List<int>();

        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < ordenes.Length; i++){
            for (int j = 0; j < ordenes[i].orders.Length; j++)
            {
                if(ordenes[i].orders[j].IDTipo_Orden != 1 && ordenes[i].orders[j].IDTipo_Orden != 2)
                {
                    if (!movids.Contains(ordenes[i].orders[j].IDOrden_en_Curso))
                    {
                        movids.Add(ordenes[i].orders[j].IDOrden_en_Curso);
                        GameObject newMov = Instantiate(movimientoPrefab, parent);
                        Movimiento mov = newMov.GetComponent<Movimiento>();
                        TileData tileOrigen = WorldManager.Instance.GetTileFromIDSpot(ordenes[i].orders[j].IDCiudadOrigen);
                        TileData tileDestino = WorldManager.Instance.GetTileFromIDSpot(ordenes[i].orders[j].IDCiudadDestino);
                        mov.tipoMov.text = tipoMovs[ordenes[i].orders[j].IDTipo_Orden - 1];
                        mov.tipoIcon.sprite = movsSprites[ordenes[i].orders[j].IDTipo_Orden - 1];
                        mov.ciudadOrigen.text = tileOrigen.nombre + "\n" + tileOrigen.ubicacion;
                        mov.ciudadDestino.text = tileDestino.nombre + "\n" + tileDestino.ubicacion;
                        mov.rondaLlegada.text = ordenes[i].orders[j].Rondafin.ToString();

                        if (tileOrigen.state == TypeSpot.barbarians)
                        {
                            mov.ownerDestino.text = tileDestino.uname != "" ? tileDestino.uname : await Web.Instance.ObtenerNombreUsuario(int.Parse(tileDestino.uid));
                            mov.ownerOrigen.text = "Barbarians";
                        }
                        else if (tileDestino.state == TypeSpot.barbarians)
                        {
                            mov.ownerDestino.text = "Barbarians";
                            mov.ownerOrigen.text = tileOrigen.uname != "" ? tileOrigen.uname : await Web.Instance.ObtenerNombreUsuario(int.Parse(tileOrigen.uid));
                        }
                        else
                        {
                            mov.ownerDestino.text = tileDestino.uname != "" ? tileDestino.uname : await Web.Instance.ObtenerNombreUsuario(int.Parse(tileDestino.uid));
                            mov.ownerOrigen.text = tileOrigen.uname != "" ? tileOrigen.uname : await Web.Instance.ObtenerNombreUsuario(int.Parse(tileOrigen.uid));
                        }

                        if (ordenes[i].orders[j].IDTipo_Orden == 4 && mov.ownerDestino.text == PlayerDataSimple.Instance.userName)
                        {
                            mov.tropa.text = "¿?";
                            mov.tipoIcon.sprite = ataqueentrante;
                        }
                        else
                        {
                            mov.tropa.text = ordenes[i].orders[j].TropasSalida.ToString();
                        }
                    }
                }  
                
            }
        }
    }

    public async void CheckAttacks()
    {
        //Debug.Log("Checking");
        PlayerDataSimple.Instance.ordenes = await Web.Instance.ObtenerOrdenes(PlayerDataSimple.Instance.userID);
        CityOrders[] missions = PlayerDataSimple.Instance.ordenes;
        for (int i = 0; i < missions.Length; i++)
        {
            for (int j = 0; j < missions[i].orders.Length; j++)
            {
                //Debug.Log("tile destino: " + missions[i].orders[j].IDCiudadDestino);
                TileData tileDestino = WorldManager.Instance.GetTileFromIDSpot(missions[i].orders[j].IDCiudadDestino);
                string defensor = tileDestino.uname != "" ? tileDestino.uname : await Web.Instance.ObtenerNombreUsuario(int.Parse(tileDestino.uid));

                if (missions[i].orders[j].IDTipo_Orden == 4 && defensor == PlayerDataSimple.Instance.userName)
                {
                    //Debug.Log("Estas bajo ataque");
                    movButton.GetComponent<Image>().color = Color.red;
                }
                else if (missions[i].orders[j].IDTipo_Orden == 4 && defensor != PlayerDataSimple.Instance.userName)
                {
                    //Debug.Log("Este es un ataque tuyo");
                    Tile attackedTile = WorldManager.Instance.GetTileSpotFromIDSpot(missions[i].orders[j].IDCiudadDestino);
                    attackedTile.actionSprite.GetComponent<SpriteRenderer>().sprite = WorldRenderer.Instance.cityAttacked;
                    attackedTile.actionSprite.SetActive(true);
                    //Debug.Log(attackedTile.name);
                }
                else
                {
                    //Debug.Log("No estas bajo ataque");
                    movButton.GetComponent<Image>().color = Color.white;
                }
            }
        }
    }
 
}
