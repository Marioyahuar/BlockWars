using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Avisos : MonoBehaviour
{
    public Text Aviso;
    public void SetAviso(string aviso)
    {
        Aviso.text = aviso;
    }

}
