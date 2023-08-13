using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    ResourceManager resourceManager;
    UIManager uiManager;

    GameObject buildingToBuild;

    private List<Building> buildings;
    private DestroyedListener destroyedListener;

    // Start is called before the first frame update
    void Awake()
    {
        buildings = new List<Building>();
        resourceManager = GetComponent<ResourceManager>();
        uiManager = GetComponent<UIManager>();
        destroyedListener = BuildingDestroyed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildingDestroyed(IDestroyable destroyedBuilding)
    {
        RemoveBuilding(destroyedBuilding as Building);
    }

    public void RemoveBuilding(Building building)
    {
        buildings.Remove(building);
    }

    public void AddBuilding(Building building)
    {
        if(building.GetBuildingType() == Building.BuildingType.Monument)
        {
            GameController.isMonumentBuild = true;
        }
        buildings.Add(building);
        building.AddDestroyedListener(destroyedListener);
    }

    public List<Building> GetAllBurnableBuildings()
    {
        List<Building> burnableBuildings = new List<Building>();
        foreach(Building building in buildings)
        {
            if (building.IsBurnable() && !building.IsBurning())
                burnableBuildings.Add(building);
        }
        return burnableBuildings;
    }

    public List<Building> GetAllBuildings()
    {
        return buildings;
    }

    public List<Building> GetAllRaidableBuildings()
    {
        List<Building> raidableBuildings = new List<Building>();
        foreach (Building building in buildings)
        {
            if (building.IsRaidable() && !building.IsBurning())
                raidableBuildings.Add(building);
        }
        return raidableBuildings;
    }

    public bool HasBuildingToBuild()
    {
        return buildingToBuild != null;
    }

    public GameObject GetBuildingToBuild()
    {
        GameObject tmp = buildingToBuild;
        buildingToBuild = null;
        return tmp;
    }

    public void SelectBuilding(GameObject buildingPrefab)
    {
        Building building = buildingPrefab.GetComponent<Building>();
        if (building == null) return;
        if (CanBuildBuilding(building))
        {
            buildingToBuild = buildingPrefab;
        }
    }

    public bool CanBuildBuilding(Building building)
    {
        List<Resource> alreadyTakenResources = new List<Resource>();
        foreach(Resource resource in building.GetBuildCost())
        {
            if (resourceManager.GetResource(resource))
            {
                alreadyTakenResources.Add(resource);
            }
            else
            {
                foreach (Resource takenResource in alreadyTakenResources)
                {
                    resourceManager.AddResource(takenResource);
                }
                return false;
            }
        }
        return true;
    }
}
