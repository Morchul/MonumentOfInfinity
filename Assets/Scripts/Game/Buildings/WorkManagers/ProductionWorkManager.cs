using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProductionWorkManager : WorkManager
{

    protected float gatherInterval;
    protected float gatherTimer;
    protected Resource product;
    protected Resource[] needs;
    protected WorkBuilding workBuilding;

    public override void Init(IWorkerAssignable workPlace)
    {
        
        if(workPlace is WorkBuilding)
        {
            base.Init(workPlace);
            workBuilding = workPlace as WorkBuilding;
            gatherInterval = workBuilding.GetGatherInteval();
            product = workBuilding.GetProduct();
            needs = workBuilding.GetNeeds();
        }
        else
        {
            throw new ArgumentException("ProductionWorkManager can only be created with a WorkBuilding");
        }
        
    }
    
}
