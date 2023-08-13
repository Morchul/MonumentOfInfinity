using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour
{

    [SerializeField] Text buildingInformation;
    [SerializeField] SmallResourceDisplayController firstBuildCost;
    [SerializeField] SmallResourceDisplayController secondBuildCost;
    [SerializeField] SmallResourceDisplayController thirdBuildCost;
    [SerializeField] SmallResourceDisplayController fourthBuildCost;
    [SerializeField] SmallResourceDisplayController firstNeed;
    [SerializeField] SmallResourceDisplayController secondNeed;
    [SerializeField] SmallResourceDisplayController thirdNeed;
    [SerializeField] SmallResourceDisplayController firstProduct;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetBuildCost(Resource[] buildCost)
    {
        SetResource(firstBuildCost, buildCost, 0);
        SetResource(secondBuildCost, buildCost, 1);
        SetResource(thirdBuildCost, buildCost, 2);
        SetResource(fourthBuildCost, buildCost, 3);
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
        } else
        {
            firstProduct.gameObject.SetActive(false);
        }
    }

    private void SetBuildingInformation(Building building)
    {
        buildingInformation.text = building.GetName() + "\nLevel: " + building.GetLevel();
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

    public void ShowTooltip(Building building)
    {
        ShowTooltip();
        SetBuildingInformation(building);
        SetBuildCost(building.GetBuildCost());
        SetNeeds(building.GetNeeds());

        if (building is WorkBuilding)
        {
            WorkBuilding workBuilding = building as WorkBuilding;
            SetProduct(workBuilding.GetProduct());
        }
        else
        {
            firstProduct.gameObject.SetActive(false);
        }
    }

    public void ShowTooltip()
    {
        this.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        this.gameObject.SetActive(false);
    }
}
