using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultProductionWorkManager : ProductionWorkManager
{

    public override void ForceToWorkBuilding()
    {
        
    }

    public override void Work()
    {
        gatherTimer += Time.deltaTime;

        if (gatherTimer > gatherInterval)
        {
            bool enoughResources = true;
            List<Resource> usedResources = new List<Resource>();
            foreach (Resource need in needs)
            {
                if (resourceManager.GetResource(need))
                {
                    usedResources.Add(need);
                }
                else
                {
                    enoughResources = false;
                    break;
                }
            }

            if (enoughResources)
            {
                resourceManager.AddResource(product);
            }
            else
            {
                foreach(Resource usedResource in usedResources)
                {
                    resourceManager.AddResource(usedResource);
                }
            }
            gatherTimer = 0;
        }
    }
}
