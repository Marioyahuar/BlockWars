using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportesUI : MonoBehaviour
{
    public Transform parent;
    public GameObject reportPrefab;
    public Sprite ataqueVictoria;
    public Sprite ataqueDerrota;
    public Sprite defensaVictoria;
    public Sprite defensaDerrota;
    public Color colorVictory;
    public Color colorDefeat;

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

            Color textColor = Color.black;
            Sprite state1 = null;
            Sprite state2 = null;

            if (reportes[i].Resultado == PlayerDataSimple.Instance.userID)
            {
                //Usuario ganador
                textColor = colorVictory;
                state1 = ataqueVictoria;
                state2 = defensaVictoria;
            }
            else
            {
                //usuario perdedor
                textColor = colorDefeat;
                state1 = ataqueDerrota;
                state2 = defensaDerrota;
            }

            report.reporte = reportes[i];
            report.ronda.text = reportes[i].Ronda.ToString();
            report.ronda.color = textColor;
            report.lugarbatalla.text = tileDestino.nombre + "\n" + tileDestino.ubicacion;
            report.lugarbatalla.color = textColor;

            //report.ganador.text = await Web.Instance.ObtenerNombreUsuario(reportes[i].Resultado);//.ToString();
            //report.atacante.text = await Web.Instance.ObtenerNombreUsuario(reportes[i].IDAtacante);
            if (tileDestino.state == TypeSpot.barbarians)
            {
                report.defensor.text = "Barbarians";
            }
            else
            {
                report.defensor.text = await Web.Instance.ObtenerNombreUsuario(reportes[i].IDDefensor);
            }
            report.defensor.color = textColor;

            if (reportes[i].IDDefensor == PlayerDataSimple.Instance.userID)
            {
                report.tipoIcon.sprite = state2;
            }
            else
            {
                report.tipoIcon.sprite = state1;
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
