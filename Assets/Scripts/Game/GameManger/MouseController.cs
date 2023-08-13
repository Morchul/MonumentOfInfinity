using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{

    [SerializeField] Button destroyButton;

    BuildingManager buildingManager;
    ResourceManager resourceManager;
    UIManager uiManager;
    GridManager gridManager;

    private GridCell gridCell;
    public static Vector3 cursorPosition { get; private set; }
    private RaycastHit hit;

    private GameObject buildingToBuild;
    private bool destroyMode;
    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GetComponent<BuildingManager>();
        resourceManager = GetComponent<ResourceManager>();
        uiManager = GetComponent<UIManager>();
        gridManager = GetComponent<GridManager>();
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            cursorPosition = hit.point;
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (buildingManager.HasBuildingToBuild())
        {
            GameObject buildingPrefab = buildingManager.GetBuildingToBuild();
            buildingToBuild = Instantiate(buildingPrefab, hit.point + new Vector3(0,0,0), Quaternion.identity);
        }

        if(buildingToBuild != null)
        {
            gridCell = gridManager.GetNearestGridCell(cursorPosition);
            buildingToBuild.transform.position = new Vector3(gridCell.centerPos.x, gridCell.centerPos.y, gridCell.centerPos.z);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                LeftClick();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
                RightClick();
        }

    }

    public float Snap(float number, float snap)
    {
        float i = number % snap;
        return i < (snap/2) ? number - i : number + (snap - i);
    }

    public void StartDestroyMode()
    {
        destroyMode = true;
        uiManager.HideAll();
        destroyButton.GetComponent<Image>().color = Color.red;
    }
    public void StopDestroyMode()
    {
        destroyMode = false;
        destroyButton.GetComponent<Image>().color = Color.white;
    }

    void LeftClick()
    {
        if(buildingToBuild != null)
        {
            TryBuild();
        }
        else if (destroyMode)
        {

            GridCell cell = gridManager.GetNearestGridCell(cursorPosition);
            if (!cell.IsFree())
            {
                if (cell.GetPlacedObject() is IPlayerDestroyable)
                {
                    if(cell.GetPlacedObject() is Building)
                    {
                        Building building = cell.GetPlacedObject() as Building;
                        if (!building.IsBurning())
                        {
                            foreach(Resource resource in building.GetBuildCost())
                            {
                                resourceManager.AddResource(resource.Type, resource.Amount / 2);
                            }
                        }
                    }

                    Destroy(cell.GetPlacedObject().GetGameObject());
                }
            }
        }
        else
        {

            if (hit.collider != null && hit.collider.gameObject.tag == "Worker")
            {
                Worker worker = hit.collider.gameObject.GetComponent<Worker>();
                if (!worker.IsState(Worker.WorkerState.AtHome) && !worker.IsState(Worker.WorkerState.AtWork))
                {
                    Debug.Log("Hit Worker");
                    uiManager.HideBuildingInformation();
                    //Maybe show worker information 
                }

            }
            else {
                GridCell cell = gridManager.GetNearestGridCell(cursorPosition);
                if (cell.HasEventObject())
                {
                    PlaceableEvent placeableEvent = cell.GetPlacedEvent();
                    if(placeableEvent != null)
                    {
                        uiManager.ShowEventInformation(placeableEvent);
                    }
                }
                else if (!cell.IsFree())
                {
                    Building building = cell.GetPlacedObject().GetGameObject().GetComponent<Building>();
                    if (building != null)
                    {
                        uiManager.ShowBuildingInformation(building);
                    }
                }
            }
        }
    }

    void RightClick()
    {
        uiManager.HideAll();
        if(buildingToBuild != null)
        {
            CancelBuilding();
        }

        if (destroyMode)
        {
            StopDestroyMode();
        }
    }

    void TryBuild()
    {
        if (hit.collider.gameObject.name == "Plane" && gridCell.IsFree())
        {
            Building building = buildingToBuild.GetComponent<Building>();
            building.Init(gridCell);
            buildingManager.AddBuilding(building);
            buildingToBuild = null;
        }
        else
        {
            uiManager.ShowMessage("Not enough Space");
        }
    }

    void CancelBuilding()
    {
        Building building = buildingToBuild.GetComponent<Building>();
        foreach(Resource resource in building.GetBuildCost())
        {
            resourceManager.AddResource(resource);
        }
        Destroy(buildingToBuild);
        buildingToBuild = null;
    }
}
