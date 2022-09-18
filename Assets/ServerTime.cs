using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using UnityEngine.Networking;

public class ServerTime : GenericSingleton<ServerTime>
{
    public ServerInfo InicializacionServerInfo;
    public ServerInfo ultimaConsultaServerInfo;
    public DateTime initServerTime;
    public DateTime lastqueryTime;
    public DateTime nextRound;
    public DateTime currentTime;

    public Text textCurrentTime;
    public Text textCurrentRound;
    public Text textNextRoundTime;
    public Text textTiempoParaIniciar;

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
        Application.runInBackground = true;
        ultimaConsultaServerInfo = await Web.ObtenerServerInfo();
        currentTime = lastqueryTime = DateTime.Parse(await Web.ObtenerHoraServer());
        nextRound = CalcularNextRoundTime(lastqueryTime);
        Debug.Log("lastqueryTimeserver=" + lastqueryTime);
    }

    public void InicializarServer()
    {
        DateTime time = DateTime.UtcNow;
        DateTime newTime = time.AddSeconds(30);
        Debug.Log(time.ToString("yyyy/MM/dd HH:mm:ss"));
        Debug.Log(newTime.ToString("yyyy/MM/dd HH:mm:ss"));
        string horaInicio = newTime.ToString("yyyy/MM/dd HH:mm:ss").Replace("/", "-");
        Debug.Log(horaInicio);


        InicializacionServerInfo.TiempoInicializado = horaInicio;
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
        Debug.Log(time.ToString("HH:mm:ss"));
        return time.ToString("HH:mm:ss");
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
                textCurrentTime.text = "Hora Actual: " + DatetimeToText(currentTime);
                //Debug.Log("time server:[" + currentTime.TimeOfDay);
                //if (currentTime.Minute % 1 == 0 && currentTime.Second == 0)
                if (currentTime >= nextRound)
                {
                    //ShowCurrentRound();
                    nextRound = CalcularNextRoundTime(currentTime);
                    textNextRoundTime.text = "Prox Ronda: " + DatetimeToText(nextRound);
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
                            textCurrentRound.text = "RONDA: " + www.downloadHandler.text;
                            Debug.Log(www.downloadHandler.text);
                            byte[] results = www.downloadHandler.data;
                            Debug.Log(results);
                        }
                    }
                }
            }
            else
            {
                string tiempoespera = (initServerTime - currentTime).ToString("ss");
                textTiempoParaIniciar.text = tiempoespera;
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