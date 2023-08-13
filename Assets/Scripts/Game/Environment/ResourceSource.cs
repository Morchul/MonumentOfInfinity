using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : MonoBehaviour, IPlaceable
{
    private GridCell cell;
    private bool destroy, isDestroyed;
    [SerializeField] Resource resource;
    EnvironmentManager environmentManager;

    // Start is called before the first frame update
    void Start()
    {
        destroy = isDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy && !isDestroyed)
        {
            isDestroyed = true;
            cell.RemoveObject();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(environmentManager != null)
            environmentManager.RemoveResourceSource(this);
    }

    public void SetEnvironmentManager(EnvironmentManager environmentManager)
    {
        this.environmentManager = environmentManager;
    }

    public void Init(GridCell cell)
    {
        this.cell = cell;
        this.cell.PlaceObject(this);
    }

    public ResourceType GetResourceType()
    {
        return resource.Type;
    }

    public GridCell GetCell()
    {
        return cell;
    }

    public int GetResource()
    {
        return resource.Amount;
    }

    public void GatherResource(int amount)
    {
        resource.Amount -= amount;
        if (resource.Amount <= 0)
            destroy = true;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
