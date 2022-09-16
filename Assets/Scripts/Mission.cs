using System;
using UnityEngine;

[System.Serializable]
public struct Mission
{
    [SerializeField] Vector3Int from;
    [SerializeField] Vector3Int to;
    [SerializeField] DateTime mission_start;
    [SerializeField] MissionType missionType;
    [SerializeField] int troopSize;


    public Mission(
        Vector3Int from,
        Vector3Int to,
        DateTime mission_start,
        MissionType missionType,
        int troopSize)
    {
        this.from = from;
        this.to = to;
        this.mission_start = mission_start;
        this.missionType = missionType;
        this.troopSize = troopSize;
    }

    public Vector3Int From { get => from; }
    public Vector3Int To { get => to; }

    public DateTime GetMissionStartTime()
    {
        return mission_start;
    }
    public MissionType GetMissionType()
    {
        return missionType;
    }

    public int GetMissionTroopSize()
    {
        return troopSize;
    }

}
public enum MissionType
{
    SendTroop,
    DefendCity,
    AttackCity
}
