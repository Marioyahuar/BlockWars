using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovimientosUI : MonoBehaviour
{
    public Transform parent;
    public GameObject movimientoPrefab;
    public GameObject movButton;

    public async void RefreshMovimientosPanel(CityOrders[] ordenes)
    {
        foreach(Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < ordenes.Length; i++){
            for (int j = 0; j < ordenes[i].orders.Length; j++)
            {
                Debug.Log("VerMovs");
                GameObject newMov = Instantiate(movimientoPrefab,parent);
                Movimiento mov = newMov.GetComponent<Movimiento>();
                TileData tileOrigen = WorldManager.Instance.GetTileFromIDSpot(ordenes[i].orders[j].IDCiudadOrigen);
                TileData tileDestino = WorldManager.Instance.GetTileFromIDSpot(ordenes[i].orders[j].IDCiudadDestino);
                mov.tipoMov.text = ordenes[i].orders[j].IDTipo_Orden.ToString();
                mov.ciudadOrigen.text = tileOrigen.nombre + "\n" + tileOrigen.ubicacion;
                mov.ciudadDestino.text = tileDestino.nombre + "\n" + tileDestino.ubicacion;
                mov.rondaLlegada.text = ordenes[i].orders[j].Rondafin.ToString();                

                if (tileOrigen.state == TypeSpot.barbarians)
                {
                    mov.ownerDestino.text = tileDestino.uname != "" ? tileDestino.uname : await Web.Instance.ObtenerNombreUsuario(int.Parse(tileDestino.uid));
                    mov.ownerOrigen.text = "Bárbaros";
                }
                else if (tileDestino.state == TypeSpot.barbarians)
                {
                    mov.ownerDestino.text = "Bárbaros";
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
                }
                else
                {
                    mov.tropa.text = ordenes[i].orders[j].TropasSalida.ToString();
                }
            }
        }
    }

    public async void CheckAttacks()
    {
        PlayerDataSimple.Instance.ordenes = await Web.Instance.ObtenerOrdenes(PlayerDataSimple.Instance.userID);
        CityOrders[] missions = PlayerDataSimple.Instance.ordenes;
        for (int i = 0; i < missions.Length; i++)
        {
            for (int j = 0; j < missions[i].orders.Length; j++)
            {
                Debug.Log("tile destino: " + missions[i].orders[j].IDCiudadDestino);
                TileData tileDestino = WorldManager.Instance.GetTileFromIDSpot(missions[i].orders[j].IDCiudadDestino);
                string defensor = tileDestino.uname != "" ? tileDestino.uname : await Web.Instance.ObtenerNombreUsuario(int.Parse(tileDestino.uid));

                if (missions[i].orders[j].IDTipo_Orden == 4 && defensor == PlayerDataSimple.Instance.userName)
                {
                    Debug.Log("Estas bajo ataque");
                    movButton.GetComponent<Image>().color = Color.red;
                }
                else
                {
                    Debug.Log("No estas bajo ataque");
                    movButton.GetComponent<Image>().color = Color.white;
                }
            }
        }
    }
    /*
    public Text tipoMov;
    public Text ciudadOrigen;
    public Text ownerOrigen;
    public Text ciudadDestino;
    public Text ownerDestino;
    public Text rondaLlegada;
    public Text tropa;
     */
}
