using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{

    UIManager uiManager;

    //lvl 0 Resources
    private int Wood;
    private int Stone;
    private int IronOre;
    private int Coal;
    private int Money;
    private int Grain;

    //lvl 1 Resources
    private int Iron; //ironore + coal
    private int Wool; //grain
    private int Milk; //grain
    private int Bread; //grain

    //lvl 2 Resources
    private int Weapon; //iron + coal
    private int Cloth; //wool
    private int Cheese; //milk

    //lvl 3 Resources
    private int TecnologyPoint;
    private int MonumentPlan; //wood + stone + money + iron

    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
    }

    private void Start()
    {
        Wood = 100;
        Stone = 100;
        Grain = 50;
        Bread = 20;
        Cloth = 10;
        Cheese = 10;
        Weapon = 10;
        Money = 100;

        //Set Start Resources here
    }

    public int GetResource(ResourceType type)
    {
        switch (type)
        {
            case ResourceType.Wood: return Wood;
            case ResourceType.Stone: return Stone;
            case ResourceType.IronOre: return IronOre;
            case ResourceType.Coal: return Coal;
            case ResourceType.Money: return Money;
            case ResourceType.Grain: return Grain;
            case ResourceType.Iron: return Iron;
            case ResourceType.Wool: return Wool;
            case ResourceType.Milk: return Milk;
            case ResourceType.Bread: return Bread;
            case ResourceType.Weapon: return Weapon;
            case ResourceType.Cloth: return Cloth;
            case ResourceType.Cheese: return Cheese;
            case ResourceType.TecnologyPoint: return TecnologyPoint;
            case ResourceType.MonumentPlan: return MonumentPlan;
            default: return 0;
        }
    }

    public bool GetResource(Resource resource)
    {
        return GetResource(resource.Type, resource.Amount);
    }

    public bool GetResource(ResourceType type, int amount)
    {
        bool returnVal;
        switch (type)
        {
            case ResourceType.Wood: returnVal = GetResource(ref Wood, amount); break;
            case ResourceType.Stone: returnVal = GetResource(ref Stone, amount); break;
            case ResourceType.IronOre: returnVal = GetResource(ref IronOre, amount); break;
            case ResourceType.Coal: returnVal = GetResource(ref Coal, amount);  break;
            case ResourceType.Money: returnVal = GetResource(ref Money, amount); break;
            case ResourceType.Grain: returnVal = GetResource(ref Grain, amount); break;
            case ResourceType.Iron: returnVal = GetResource(ref Iron, amount); break;
            case ResourceType.Wool: returnVal = GetResource(ref Wool, amount); break;
            case ResourceType.Milk: returnVal = GetResource(ref Milk, amount); break;
            case ResourceType.Bread: returnVal = GetResource(ref Bread, amount); break;
            case ResourceType.Weapon: returnVal = GetResource(ref Weapon, amount); break;
            case ResourceType.Cloth: returnVal = GetResource(ref Cloth, amount); break;
            case ResourceType.Cheese: returnVal = GetResource(ref Cheese, amount); break;
            case ResourceType.TecnologyPoint: returnVal = GetResource(ref TecnologyPoint, amount); break;
            case ResourceType.MonumentPlan: returnVal = GetResource(ref MonumentPlan, amount); break;
            default: return false;
        }

        if (!returnVal)
        {
            uiManager.ShowMessage("Not enough " + type.ToString());
        }
        return returnVal;
    }

    public void AddResource(Resource resource)
    {
        AddResource(resource.Type, resource.Amount);
    }

    public void AddResource(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood: AddResource(ref Wood, amount); break;
            case ResourceType.Stone: AddResource(ref Stone, amount); break;
            case ResourceType.IronOre: AddResource(ref IronOre, amount); break;
            case ResourceType.Coal: AddResource(ref Coal, amount); break;
            case ResourceType.Money: AddResource(ref Money, amount); break;
            case ResourceType.Grain: AddResource(ref Grain, amount); break;
            case ResourceType.Iron: AddResource(ref Iron, amount); break;
            case ResourceType.Wool: AddResource(ref Wool, amount); break;
            case ResourceType.Milk: AddResource(ref Milk, amount); break;
            case ResourceType.Bread: AddResource(ref Bread, amount); break;
            case ResourceType.Weapon: AddResource(ref Weapon, amount); break;
            case ResourceType.Cloth: AddResource(ref Cloth, amount); break;
            case ResourceType.Cheese: AddResource(ref Cheese, amount); break;
            case ResourceType.TecnologyPoint: AddResource(ref TecnologyPoint, amount); break;
            case ResourceType.MonumentPlan: AddResource(ref MonumentPlan, amount); break;
        }
    }


    private bool GetResource(ref int resource, int amount)
    {
        if (resource < amount)
        {
            return false;
        }
        resource -= amount;
        return true;
    }
    private void AddResource(ref int resource, int amount)
    {
        resource += amount;
    }
}
