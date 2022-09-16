using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ServerTime : MonoBehaviour
{
    public ServerInfo InicializacionServerInfo;
    public ServerInfo ultimaConsultaServerInfo;
    public DateTime initServerTime;
    public DateTime lastqueryTime;
    public DateTime nextRound;
    public DateTime currentTime;
    public InputField horaInicio;

    public Text textCurrentTime;
    public Text textCurrentRound;
    public Text textNextRoundTime;

    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long ConvertToTimestamp(DateTime value)
    {
        TimeSpan elapsedTime = value - Epoch;
        return (long)elapsedTime.TotalSeconds;
    }
    public static DateTime ConvertFromTimestamp(long timestamp)
    {
        return Epoch.AddSeconds(timestamp);
    }

    private async void Start()
    {
        ultimaConsultaServerInfo = await Web.ObtenerServerInfo();
        currentTime = lastqueryTime = DateTime.Parse(await Web.ObtenerHoraServer());
        nextRound = CalcularNextRoundTime(lastqueryTime);
        Debug.Log("lastqueryTimeserver=" + lastqueryTime);
    }

    public void InicializarServer()
    {
        InicializacionServerInfo.TiempoInicializado = "2022-09-16 " + horaInicio.text;
        StartCoroutine(Web.InicializarServer(InicializacionServerInfo));
    }

    public async void CorrerTiempo()
    {
        ultimaConsultaServerInfo = await Web.ObtenerServerInfo();
        initServerTime = DateTime.Parse(ultimaConsultaServerInfo.TiempoInicializado);
        lastqueryTime = currentTime = DateTime.Parse(await Web.ObtenerHoraServer());
        nextRound = CalcularNextRoundTime(lastqueryTime);
        StartCoroutine(runTime());
    }

    private string DatetimeToText(DateTime time)
    {
        return time.TimeOfDay.ToString();
    }

    IEnumerator  runTime()
    {
        while (true)
        {

            yield return new WaitForSeconds(1);
            //currentTime = currentTime.AddSeconds(1);
            currentTime = DateTime.UtcNow;
            if (!(currentTime < initServerTime))
            {
                textCurrentTime.text = DatetimeToText(currentTime);
                Debug.Log("time server:[" + currentTime.TimeOfDay);
                //if (currentTime.Minute % 1 == 0 && currentTime.Second == 0)
                if (currentTime >= nextRound)
                {
                    //ShowCurrentRound();
                    nextRound = CalcularNextRoundTime(currentTime);
                    textNextRoundTime.text = DatetimeToText(nextRound);
                    //ejecutar comando de actualizacion;
                    using (UnityWebRequest www = UnityWebRequest.Get("https://block-chest.com/PixelWars/ejecutarRonda.php?"))
                    {
                        yield return www.SendWebRequest();

                        if (www.isNetworkError || www.isHttpError)
                        {
                            Debug.Log(www.error);
                        }
                        else
                        {
                            textCurrentRound.text = www.downloadHandler.text;
                            Debug.Log(www.downloadHandler.text);
                            byte[] results = www.downloadHandler.data;
                            Debug.Log(results);
                        }
                    }
                }
            }
            else
            {
                Debug.Log("en espera");
            }
        }
    }

    private DateTime CalcularNextRoundTime(DateTime lastTime)
    {
        long timestamp = ConvertToTimestamp(lastTime);
        int duracionRondaenSeg = (ultimaConsultaServerInfo.Duracion * 60);
        long resto = timestamp % duracionRondaenSeg;
        long timestampSigRonda = (timestamp - resto + duracionRondaenSeg);
        var next = ConvertFromTimestamp(timestampSigRonda);
        Debug.Log("next round time = " + next);
        return next;
    }

    public int[] ShowCurrentRound()//jalar esta funcion para obtener 
    {
        int lastserverRound = ultimaConsultaServerInfo.Ronda;
        TimeSpan timespan = currentTime - lastqueryTime;
        Debug.Log("timespan: " + timespan);
        float r = (float)timespan.TotalSeconds / (ultimaConsultaServerInfo.Duracion * 60);
        int rondaActual = ultimaConsultaServerInfo.Ronda + (int)r;
        Debug.Log("rondaactual: "+ rondaActual);
        var tiempoFaltanteParalaSiguienteRonda = nextRound - currentTime;
        Debug.Log("segundos"+ ((int)tiempoFaltanteParalaSiguienteRonda.TotalSeconds).ToString());
        int[] result = { rondaActual, (int)tiempoFaltanteParalaSiguienteRonda.TotalSeconds };
        return result;
    }
}