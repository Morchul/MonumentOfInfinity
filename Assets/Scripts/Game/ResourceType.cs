public enum ResourceType
{
    Wood,
    Stone,
    IronOre,
    Coal,
    Money,
    Grain,
    Iron,
    Wool,
    Milk,
    Bread,
    Weapon,
    Cloth,
    Cheese,
    TecnologyPoint,
    MonumentPlan
}

[System.Serializable]
public struct Resource
{
    public ResourceType Type;
    public int Amount;

    public Resource(ResourceType type, int amount)
    {
        this.Type = type;
        this.Amount = amount;
    }
}
