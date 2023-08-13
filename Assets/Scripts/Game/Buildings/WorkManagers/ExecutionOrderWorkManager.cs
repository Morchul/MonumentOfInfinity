using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ExecutionOrderWorkManager : ProductionWorkManager
{
    protected Queue<Func<bool>> executionOrder;
    protected Func<bool> currentExecution;
    protected GridCell targetCell;
    protected List<Vector3> way;
    protected int nextWayPoint;
    protected int gatheredResources;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        executionOrder = new Queue<Func<bool>>();
    }

    protected override void RemoveWorkerFinally()
    {
        base.RemoveWorkerFinally();
        executionOrder.Clear();
        way = null;
        gatheredResources = 0;
        nextWayPoint = -1;
        currentExecution = null;
    }

    protected void ExecuteExecutionOrder()
    {
        if (currentExecution == null)
            currentExecution = executionOrder.Dequeue();

        if (currentExecution()) //step finished
        {
            currentExecution = null;
            if (executionOrder.Count == 0)
            {
                ExecutionFinished();
            }
        }
    }

    protected bool MoveToTargetCell()
    {
        if (way == null)
            way = WayManager.FindWay(workBuilding.GetCell(), targetCell);
        //move way
        if (nextWayPoint < 0)
            nextWayPoint = 0;
        Vector3 directionVec = way[nextWayPoint] - worker.gameObject.transform.position;
        if ((directionVec).magnitude < 0.1f)
        {
            //reached
            if (++nextWayPoint == way.Count)
            {
                nextWayPoint = -1;
                way = null;
                return true;
            }
        }
        else
        {
            worker.Move(directionVec);
        }
        //walk

        return false;

    }

    protected bool BackToWorkBuilding()
    {
        if (way == null)
            way = WayManager.FindWay(targetCell, workBuilding.GetCell());
        //move way
        if (nextWayPoint < 0)
            nextWayPoint = 0;
        Vector3 directionVec = way[nextWayPoint] - worker.gameObject.transform.position;
        if ((directionVec).magnitude < 0.1f)
        {
            //reached
            if (++nextWayPoint == way.Count)
            {
                if(!destroy)
                    resourceManager.AddResource(workBuilding.GetProduct().Type, gatheredResources);
                nextWayPoint = -1;
                way = null;
                return true;
            }
        }
        //walk
        worker.Move(directionVec);
        return false;
    }

    protected abstract void ExecutionFinished();
}
