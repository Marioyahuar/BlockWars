using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour
{
    public GameObject Options;
    public Text userName;
    public Text villageCoord;
    public Text nivel;
    public Text nivelMax;
    public Text distance;
    public Text tropa;
    public Button AtackBTN;
    public Button SendBTN;
    public Button ConquerBTN;

    public void OpenOptions()
    {
        Options.SetActive(true);
    }

    public void CloseOptions()
    {
        Options.SetActive(false);
    }
}
