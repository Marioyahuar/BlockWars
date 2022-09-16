using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct User
{
    [SerializeField] private string uid;
    [SerializeField] private string uname;

    public User(string uid, string uname)
    {
        this.uid = uid;
        this.uname = uname;
    }
    public string GetUname()
    {
        return uname;
    }
    public string GetUid()
    {
        return uid;
    }

    public static User Empty()
    {
        return new User("", "");
    }
}
