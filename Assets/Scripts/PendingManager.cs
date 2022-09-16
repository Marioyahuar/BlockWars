using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendingManager : GenericSingleton<PendingManager>
{
    public List<Mission> pendingMissions;
    public List<TroopCreation> pendingTroopCreations;
    public List<CityUpgrade> pendingCityUpgrade;

    public void AddPendingTroopCreation(string uid, int cityID, int troop)
    {
        pendingTroopCreations.Add(new TroopCreation(uid, cityID, troop));
    }

    public void AddPendingMission(MissionType missionType, Vector3Int coordInit, Vector3Int coordFin, int troop)
    {

    }

    public void AddPendingCityUpgrade(string uid, int cityID, int lvl)
    {
        pendingCityUpgrade.Add(new CityUpgrade(uid, cityID, lvl));
    }
}
