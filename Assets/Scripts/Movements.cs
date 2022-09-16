using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Movements
{
    [SerializeField] List<Mission> missions;

    public Movements(List<Mission> missions)
    {
        this.missions = missions;
    }

    public static List<Mission> ListMission()
    {
        return new List<Mission>();
    }
    public List<Mission> GetMissions()
    {
        return missions;
    }
    public void AddMission(Mission mission)
    {
        missions.Add(mission);
    }
    public void RemoveMission(int index)
    {
        missions.RemoveAt(index);
    }
}