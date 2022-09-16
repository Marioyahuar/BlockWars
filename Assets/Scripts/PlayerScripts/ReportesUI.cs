using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportesUI : MonoBehaviour
{
    public Transform parent;
    public GameObject reportPrefab;

    public async void RefreshReportesPanel(Reporte[] reportes)
    {
        
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < reportes.Length; i++)
        {
            GameObject newReport = Instantiate(reportPrefab, parent);
            Report report = newReport.GetComponent<Report>();
            Order order = await Web.Instance.ObtenerOrdenPorID(reportes[i].IDOrdenEncurso);
            TileData tileDestino = WorldManager.Instance.GetTileFromIDSpot(order.IDCiudadDestino);
            report.reporte = reportes[i];
            report.ronda.text = reportes[i].Ronda.ToString();
            report.lugarbatalla.text = tileDestino.nombre + "\n" + tileDestino.ubicacion;
            report.ganador.text = await Web.Instance.ObtenerNombreUsuario(reportes[i].Resultado);//.ToString();
            report.atacante.text = await Web.Instance.ObtenerNombreUsuario(reportes[i].IDAtacante);
            if (tileDestino.state == TypeSpot.barbarians)
            {
                report.defensor.text = "Bárbaros";
            }
            else
            {
                report.defensor.text = await Web.Instance.ObtenerNombreUsuario(reportes[i].IDDefensor);
            }
            

        }
    }

    /*
    public Text ronda;
    public Text atacante;
    public Text defensor;
    public Text lugarbatalla;
    public Text ganador;
     */
}
