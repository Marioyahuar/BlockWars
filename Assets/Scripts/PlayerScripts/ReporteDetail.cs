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
            defensorUserName = "Barbarians";
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

        title.text = atacanteUserName +  " attacks " + tileDestino.nombre + "(" + tileDestino.ubicacion + ")";
        ronda.text = reporte.Ronda.ToString();
        atacanteName.text = atacanteUserName;
        defensorName.text = defensorUserName;
        atacanteTropas.text = order.TropasSalida.ToString() + " troops" + "(-" + reporte.PerdidaAtaque + ")";
        defensorTropas.text = reporte.Defensores + " troops" + "(-" + reporte.PerdidaDefensa + ")";
        resultado.text = ganador + " won the battle";
        botin.text = (ganador == atacanteUserName) ? (elBotin.ToString() + " troops joined the army of " + atacanteUserName) : "The attacker has lost";
    }
}
