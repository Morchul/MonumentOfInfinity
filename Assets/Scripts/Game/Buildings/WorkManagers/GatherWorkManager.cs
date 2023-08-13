using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherWorkManager : ExecutionOrderWorkManager
{

    private ResourceSource currentResourceSource;
    private EnvironmentManager environmentManager;

    private float waitTimer, waitTime = 2f;

    private bool init = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        environmentManager = GameController.FindEnvironment().GetComponent<EnvironmentManager>();
    }

    protected override void RemoveWorkerFinally()
    {
        base.RemoveWorkerFinally();
        init = true;
        waitTimer = 0;
        currentResourceSource = null;
    }

    public override void ForceToWorkBuilding()
    {
        if (worker.IsState(Worker.WorkerState.WorkingOutside))
        {
            ExecuteExecutionOrder();
            return;
        }
    }

    public override void Work()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer > waitTime)
        {
            if (worker.IsState(Worker.WorkerState.WorkingOutside))
            {
                ExecuteExecutionOrder();
                return;
            }

            if (currentResourceSource == null || currentResourceSource.GetResource() == 0)
            {
                currentResourceSource = environmentManager.GetNearestResourceSource(workBuilding.GetProduct().Type, workPlace.GetCell());
                targetCell = currentResourceSource.GetCell();
            }
            GatherResourceExecution();
        }
    }

    protected override void ExecutionFinished()
    {
        init = true;
        waitTimer = 0;
        worker.State = Worker.WorkerState.AtWork;
    }

    private void GatherResourceExecution()
    {
        if (init)
        {
            worker.State = Worker.WorkerState.GatherResource;
            executionOrder.Enqueue(MoveToTargetCell);
            executionOrder.Enqueue(GatherResources);
            executionOrder.Enqueue(BackToWorkBuilding);
            nextWayPoint = -1;
            gatherTimer = 0;
            init = false;
        }
    }

    private bool GatherResources()
    {
        if (gatherTimer < gatherInterval)
        {
            gatherTimer += Time.deltaTime;
            return false;
        }
        gatheredResources = Mathf.Min(workBuilding.GetProduct().Amount, currentResourceSource.GetResource());
        currentResourceSource.GatherResource(gatheredResources);
        return true;
    }
}
