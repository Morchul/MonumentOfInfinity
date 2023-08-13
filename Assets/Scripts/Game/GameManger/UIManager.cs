using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    ResourceManager resources;
    BillingSystem billingSystem;

    [SerializeField] Text wood;
    [SerializeField] Text stone;
    [SerializeField] Text ironOre;
    [SerializeField] Text coal;
    [SerializeField] Text money;
    [SerializeField] Text grain;
    [SerializeField] Text iron;
    [SerializeField] Text wool;
    [SerializeField] Text milk;
    [SerializeField] Text bread;
    [SerializeField] Text weapon;
    [SerializeField] Text cloth;
    [SerializeField] Text chees;
    [SerializeField] Text tecnologyPoint;
    [SerializeField] Text monumentPlan;

    [SerializeField] GameObject smallBuildingsPanel;
    [SerializeField] GameObject mediumBuildingsPanel;
    [SerializeField] GameObject bigBuildingsPanel;
    [SerializeField] GameObject specialBuildingsPanel;

    [SerializeField] BuildingInformationController buildingInformation;
    [SerializeField] EventInformationController eventInformation;

    [SerializeField] TooltipController tooltipController;

    [SerializeField] InformationTextController informationTexts;

    [SerializeField] GameObject victoryScreen;
    [SerializeField] GameObject lostScreen;
    [SerializeField] Text lostReason;

    [SerializeField] Text loading;

    [SerializeField] Text billingInformation;

    private List<BuildingTooltip> buildingNames;
    public bool ShowBuildingNames { get; private set; }

    private void Awake()
    {
        buildingNames = new List<BuildingTooltip>();
        ShowBuildingNames = true;
    }

    public void AddBuildingName(BuildingTooltip buildingTooltip)
    {
        buildingNames.Add(buildingTooltip);
    }

    public void RemoveBuildingName(BuildingTooltip buildingTooltip)
    {
        buildingNames.Remove(buildingTooltip);
    }

    public void FinishLoading()
    {
        loading.gameObject.SetActive(false);
    }

    public void ShowBuildingNamesONOff()
    {
        ShowBuildingNames = !ShowBuildingNames;
        foreach(BuildingTooltip buildingTooltip in buildingNames)
        {
            buildingTooltip.gameObject.SetActive(ShowBuildingNames);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        resources = GetComponent<ResourceManager>();
        billingSystem = GetComponent<BillingSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateResourceTexts();


        billingInformation.text = "+" + billingSystem.GetBillingAmount() + " (" + billingSystem.GetTimeUntilBilling() + ")";


        if (GameController.isMonumentBuild)
        {
            victoryScreen.SetActive(true);
        } else if (GameController.lost)
        {
            lostReason.text = GameController.GetLostReason();
            lostScreen.SetActive(true);
        }
    }

    public void ShowBuildingInformation(Building building)
    {
        HideAll();
        buildingInformation.ShowBuildingInformation(building);
    }

    public void ShowEventInformation(Event _event)
    {
        HideAll();
        eventInformation.ShowEventInformation(_event);
    }

    public void ShowTooltip(Building building)
    {
        tooltipController.ShowTooltip(building);
    }

    public void HideTooltip()
    {
        tooltipController.HideTooltip();
    }

    public void HideBuildingInformation()
    {
        this.buildingInformation.HideBuildingInformation();
    }

    public void HideEventInformation()
    {
        this.eventInformation.HideEventInformation();
    }

    private void UpdateResourceTexts()
    {
        wood.text = resources.GetResource(ResourceType.Wood).ToString();
        stone.text = resources.GetResource(ResourceType.Stone).ToString();
        ironOre.text = resources.GetResource(ResourceType.IronOre).ToString();
        coal.text = resources.GetResource(ResourceType.Coal).ToString();
        money.text = resources.GetResource(ResourceType.Money).ToString();
        grain.text = resources.GetResource(ResourceType.Grain).ToString();
        iron.text = resources.GetResource(ResourceType.Iron).ToString();
        wool.text = resources.GetResource(ResourceType.Wool).ToString();
        milk.text = resources.GetResource(ResourceType.Milk).ToString();
        bread.text = resources.GetResource(ResourceType.Bread).ToString();
        weapon.text = resources.GetResource(ResourceType.Weapon).ToString();
        cloth.text = resources.GetResource(ResourceType.Cloth).ToString();
        chees.text = resources.GetResource(ResourceType.Cheese).ToString();
        tecnologyPoint.text = resources.GetResource(ResourceType.TecnologyPoint).ToString();
        monumentPlan.text = resources.GetResource(ResourceType.MonumentPlan).ToString();
    }

    public void OpenCloseSmallBuildings()
    {
        smallBuildingsPanel.SetActive(!smallBuildingsPanel.activeSelf);
        HideTooltip();
        HideBuildingInformation();
    }

    public void OpenCloseMediumBuildings()
    {
        mediumBuildingsPanel.SetActive(!mediumBuildingsPanel.activeSelf);
        HideTooltip();
        HideBuildingInformation();
    }

    public void OpenCloseBigBuildings()
    {
        bigBuildingsPanel.SetActive(!bigBuildingsPanel.activeSelf);
        HideTooltip();
        HideBuildingInformation();
    }

    public void OpenCloseSpecialBuildings()
    {
        specialBuildingsPanel.SetActive(!specialBuildingsPanel.activeSelf);
        HideTooltip();
        HideBuildingInformation();
    }

    public void HideAll()
    {
        CloseBuildings();
        HideBuildingInformation();
        HideEventInformation();
    }

    public void CloseBuildings()
    {
        smallBuildingsPanel.SetActive(false);
        mediumBuildingsPanel.SetActive(false);
        bigBuildingsPanel.SetActive(false);
        specialBuildingsPanel.SetActive(false);
        HideTooltip();
    }

    public void ShowMessage(string message)
    {
        informationTexts.AddInformationText(message);
    }
}
