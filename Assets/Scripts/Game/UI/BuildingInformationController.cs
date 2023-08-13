using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInformationController : MonoBehaviour
{

    [SerializeField] Text buildingName;
    [SerializeField] Text buildingLevel;

    [SerializeField] DropdownController firstWorkerChooser;
    [SerializeField] DropdownController secondWorkerChooser;

    [SerializeField] SmallResourceDisplayController firstNeed;
    [SerializeField] SmallResourceDisplayController secondNeed;
    [SerializeField] SmallResourceDisplayController thirdNeed;
    [SerializeField] SmallResourceDisplayController firstProduct;

    Building currentBuilding;
    WorkerManager workerManager;
    UIManager uiManager;

    DestroyedListener listener;

    // Start is called before the first frame update
    void Start()
    {
        firstWorkerChooser.AddListener(WorkerChooserChanged);
        secondWorkerChooser.AddListener(WorkerChooserChanged);
        GameObject gameManager = GameController.FindGameManager();
        workerManager = gameManager.GetComponent<WorkerManager>();
        uiManager = gameManager.GetComponent<UIManager>();
        listener = BuildingDestroyed;
    }

    private void BuildingDestroyed(IDestroyable _)
    {
        HideBuildingInformation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetNeeds(Resource[] needs)
    {
        SetResource(firstNeed, needs, 0);
        SetResource(secondNeed, needs, 1);
        SetResource(thirdNeed, needs, 2);
    }

    private void SetProduct(Resource product)
    {
        if (product.Amount > 0)
        {
            firstProduct.gameObject.SetActive(true);
            firstProduct.SetResource(product);
        }
        else
        {
            firstProduct.gameObject.SetActive(false);
        }
    }

    private void SetResource(SmallResourceDisplayController resourceDisplay, Resource[] resources, int index)
    {
        if (resources.Length > index)
        {
            resourceDisplay.gameObject.SetActive(true);
            resourceDisplay.SetResource(resources[index]);
        }
        else
        {
            resourceDisplay.gameObject.SetActive(false);
        }
    }

    public void WorkerChooserChanged(int value, int workerIndex)
    {
        Worker worker;
        IWorkerAssignable workBuilding = currentBuilding as IWorkerAssignable;

        if (value == 0) // no worker selected
        {
            //worker = null;
            Worker currentWorker = workBuilding.GetWorkManager(workerIndex).GetWorker();
            if (currentWorker != null)
                currentWorker.NewWorkPlace(null);
        }
        else
        {
            worker = workerManager.GetUnemployedWorker()[value - 1];
            if (workBuilding.IsLevelHighEnough(worker))
            {
                WorkManager workManager = workBuilding.GetWorkManager(workerIndex);
                Worker currentWorker = workManager.GetWorker();
                if (currentWorker != null)
                    currentWorker.NewWorkPlace(null);

                worker.NewWorkPlace(workManager); //assign new Workplace but still inactive
                workManager.AssignWorker(worker); //assign to Workmanager but still inactive
            }
            else
            {
                uiManager.ShowMessage("Workers Level is too low");
            }
        }
        //workBuilding.SetWorker(workerIndex, worker);
        UpdateWorkerChooser();
    }

    public void UpdateWorkerChooser()
    {
        firstWorkerChooser.UpdateDropdownValues();
        secondWorkerChooser.UpdateDropdownValues();
    }

    public void ShowBuildingInformation(Building buildingInformation)
    {
        if(currentBuilding != null)
        {
            currentBuilding.RemoveDestroyedListener(listener);
        }

        ShowBuildingInformation();
        currentBuilding = buildingInformation;


        if (currentBuilding != null)
        {
            //show lvl, name, etc.
            currentBuilding.AddDestroyedListener(listener);
            buildingName.text = "Name: " + currentBuilding.GetName();
            buildingLevel.text = "Level: " + currentBuilding.GetLevel();
            SetNeeds(buildingInformation.GetNeeds());

            if (currentBuilding is WorkBuilding)
            {
                WorkBuilding workBuilding = currentBuilding as WorkBuilding;
                firstWorkerChooser.gameObject.SetActive(true);
                secondWorkerChooser.gameObject.SetActive(true);
                SetProduct(workBuilding.GetProduct());
                firstWorkerChooser.SelectedWorkerAssignableChanged(workBuilding);
                secondWorkerChooser.SelectedWorkerAssignableChanged(workBuilding);

                UpdateWorkerChooser();
            }
            else
            {
                firstProduct.gameObject.SetActive(false);
                firstWorkerChooser.gameObject.SetActive(false);
                secondWorkerChooser.gameObject.SetActive(false);
            }
        }
    }

    public void HideBuildingInformation()
    {
        gameObject.SetActive(false);
    }

    public void ShowBuildingInformation()
    {
        gameObject.SetActive(true);
    }
}
