using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Order
{
    public int IDOrden_en_Curso;
    public int IDTipo_Orden;
    public int IDCiudadOrigen;
    public int IDCiudadDestino;
    public int RondaInicio;
    public int TropasSalida;
    public int Rondafin;
    public int TropasRetorno;
    public int Estado;

    public Order(int iDOrden_en_Curso, int iDTipo_Orden, int iDCiudadOrigen, int iDCiudadDestino, int rondaInicio, int tropasSalida, int rondafin, int tropasRetorno, int estado)
    {
        IDOrden_en_Curso = iDOrden_en_Curso;
        IDTipo_Orden = iDTipo_Orden;
        IDCiudadOrigen = iDCiudadOrigen;
        IDCiudadDestino = iDCiudadDestino;
        RondaInicio = rondaInicio;
        TropasSalida = tropasSalida;
        Rondafin = rondafin;
        TropasRetorno = tropasRetorno;
        Estado = estado;
    }
    public Order(Order order)
    {
        IDOrden_en_Curso = order.IDOrden_en_Curso;
        IDTipo_Orden = order.IDTipo_Orden;
        IDCiudadOrigen = order.IDCiudadOrigen;
        IDCiudadDestino = order.IDCiudadDestino;
        RondaInicio = order.RondaInicio;
        TropasSalida = order.TropasSalida;
        Rondafin = order.Rondafin;
        TropasRetorno = order.TropasRetorno;
        Estado = order.Estado;
    }
    /*
{
"IDOrden_en_Curso":2,
"IDTipo_Orden":1,
"IDCiudadOrigen":108,
"IDCiudadDestino":108,
"RondaInicio":2,
"TropasSalida":0,
"Rondafin":3,
"TropasRetorno":1,
"Estado":1
}
*/
}
