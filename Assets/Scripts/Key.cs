using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Key
{
    [SerializeField] private string publicAddress;

    public Key(string publicAddress)
    {
        if (publicAddress.Length != 42)
        {
            this.publicAddress = "";
            return;
        }
        this.publicAddress = publicAddress;
    }

    public string PublicAddress { get => publicAddress; }
}