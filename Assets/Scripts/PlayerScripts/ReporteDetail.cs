using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReporteDetail : MonoBehaviour
{
    public Text title;
    public Text ronda;
    public Text atacanteName;
    public Text atacanteTropas;
    public Text defensorName;
    public Text defensorTropas;
    public Text resultado;
    public Text botin;

    public async void RefreshReporteDetails(Reporte reporte)
    {
        string atacanteUserName = await Web.Instance.ObtenerNombreUsuario(reporte.IDAtacante);
        string defensorUserName = await Web.Instance.ObtenerNombreUsuario(reporte.IDDefensor);
        if (defensorUserName == "null")
        {
            defensorUserName = "Bárbaros";
        }
        
        string ganador = "";
        int elBotin = 0;
        if (reporte.Resultado == reporte.IDAtacante)
        {
            //ganó el atacante
            ganador = atacanteUserName;
            elBotin = reporte.PerdidaDefensa / 2;
        }
        else
        {
            //ganó el defensor
            ganador = defensorUserName;
        }
        Order order = await Web.Instance.ObtenerOrdenPorID(reporte.IDOrdenEncurso);
        TileData tileDestino = WorldManager.Instance.GetTileFromIDSpot(order.IDCiudadDestino);

        title.text = atacanteUserName +  " ataca a " + tileDestino.nombre + "(" + tileDestino.ubicacion + ")";
        ronda.text = reporte.Ronda.ToString();
        atacanteName.text = atacanteUserName;
        defensorName.text = defensorUserName;
        atacanteTropas.text = order.TropasSalida.ToString() + " tropas" + "(-" + reporte.PerdidaAtaque + ")";
        defensorTropas.text = reporte.Defensores + " tropas" + "(-" + reporte.PerdidaDefensa + ")";
        resultado.text = ganador + " ganó la batalla";
        botin.text = (ganador == atacanteUserName) ? (elBotin.ToString() + " tropas se unieron a las filas de " + atacanteUserName) : "El atacante ha perdido";
    }
}
