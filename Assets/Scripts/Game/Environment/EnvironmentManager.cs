using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    [SerializeField] ResourceSource forestPrefab;
    [SerializeField] ResourceSource stonePrefab;
    [SerializeField] ResourceSource coalPrefab;
    [SerializeField] ResourceSource ironOrePrefab;

    private Dictionary<ResourceType, List<ResourceSource>> allResourceSources;
    
    GridManager gridManager;
    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        allResourceSources = new Dictionary<ResourceType, List<ResourceSource>>();
        GenerateResourceSources(2, coalPrefab, ResourceType.Coal);
        GenerateResourceSources(2, ironOrePrefab, ResourceType.IronOre);
        GenerateResourceSources(4, stonePrefab, ResourceType.Stone);
        GenerateResourceSources(6, forestPrefab, ResourceType.Wood);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveResourceSource(ResourceSource resourceSource)
    {
        allResourceSources.TryGetValue(resourceSource.GetResourceType(), out List<ResourceSource> resourceList);
        resourceList.Remove(resourceSource);
    }

    private void GenerateResourceSources(int amount, ResourceSource resourceSourcePrefab, ResourceType type)
    {
        List<ResourceSource> resourceControllers = new List<ResourceSource>();

        for (int i = 0; i < amount; ++i)
        {
            GridCell resourceCell = gridManager.GetRandomFreeCell();
            ResourceSource resourceController = Instantiate(resourceSourcePrefab, resourceCell.centerPos, Quaternion.Euler(0,-90,0), gameObject.transform);
            resourceController.Init(resourceCell);
            resourceController.SetEnvironmentManager(this);
            resourceControllers.Add(resourceController);
        }
        allResourceSources.Add(type, resourceControllers);
    }

    public ResourceSource GetNearestResourceSource(ResourceType type, GridCell pos)
    {
        ResourceSource nearestResourceSource = null;
        float distance = 0;
        if(allResourceSources.TryGetValue(type, out List<ResourceSource> resourceSources))
        {
            foreach (ResourceSource resourceSource in resourceSources)
            {
                if (nearestResourceSource == null)
                {
                    nearestResourceSource = resourceSource;
                    distance = CalcDistance(pos, nearestResourceSource.GetCell());
                }
                else
                {
                    float distanceTmp = CalcDistance(resourceSource.GetCell(), pos);
                    if (distanceTmp < distance)
                    {
                        nearestResourceSource = resourceSource;
                        distance = distanceTmp;
                    }
                }

            }
        }
        
        return nearestResourceSource;
    }

    private float CalcDistance(GridCell pointA, GridCell pointB)
    {
        float a = Mathf.Abs(pointA.posX - pointB.posX);
        float b = Mathf.Abs(pointA.posZ - pointB.posZ);
        return Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
    }
}
