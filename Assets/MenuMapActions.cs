using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMapActions : MonoBehaviour
{
    [SerializeField] GameObject panelButtons;
    [SerializeField] GameObject[] buttons;
    [SerializeField] Text txtUserName;
    [SerializeField] Text txtUserCoord;
    [SerializeField] Text txtDistance;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextInfo(string username, string usercoord, string distance)
    {
        txtUserName.text = username;
        txtUserCoord.text = usercoord;
        txtDistance.text = distance;
    }

    public void SetButtonsMode()
    {

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
