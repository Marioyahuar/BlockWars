using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Report : MonoBehaviour
{
    public Reporte reporte;
    public Image tipoIcon;
    public Text ronda;
    public Text atacante;
    public Text defensor;
    public Text lugarbatalla;
    public Text ganador;
    public Button verReporte;

    public void OpenReporteDetail()
    {
        UIManager.Instance.VerReporteDetail(this.reporte);
    }
}