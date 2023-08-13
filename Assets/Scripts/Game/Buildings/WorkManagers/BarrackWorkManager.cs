using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrackWorkManager : WorkManager
{

    private Building building;

    public override void ForceToWorkBuilding()
    {
        
    }

    public override void Work()
    {
        
    }

    public override void Init(IWorkerAssignable workPlace)
    {
        if(workPlace is Building)
        {
            base.Init(workPlace);
            building = workPlace as Building;
        }
        else
        {
            throw new ArgumentException("WorkPlace needs to be a Building for the BarrackWorkManager");
        }
        
    }

    public override void AssignWorker(Worker worker)
    {
        base.AssignWorker(worker);
        worker.ConvertToSoldier(building, this); 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
