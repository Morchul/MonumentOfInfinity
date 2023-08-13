using System.Collections.Generic;
using UnityEngine;

public class FireEvent : Event
{

    [SerializeField] Fire FirePrefab;

    BuildingManager buildingManager;

    public override void Trigger()
    {
        if(buildingManager == null)
            buildingManager = GameController.FindGameManager().GetComponent<BuildingManager>();

        List<Building> burnableBuildings = buildingManager.GetAllBurnableBuildings();
        if (burnableBuildings.Count > 0)
        {
            Building burnBuilding = burnableBuildings[Random.Range(0, burnableBuildings.Count)];
            Fire fire = Instantiate(FirePrefab, burnBuilding.GetCell().centerPos, Quaternion.Euler(-90,0,0));
            fire.Init(burnBuilding.GetCell());
        }
    }
}
