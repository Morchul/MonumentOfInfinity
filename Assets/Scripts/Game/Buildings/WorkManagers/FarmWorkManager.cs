using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmWorkManager : ExecutionOrderWorkManager
{

    [SerializeField] GrainFieldController grainFieldPrefab;
    Transform environment;
    GridManager gridManager;

    private List<GrainFieldController> grainFields;
    private GrainFieldController currentGrainField;

    private bool init = true;

    private float waitTimer, waitTime = 1f;
    private float grainFieldPlantTimer, grainFieldGatherTimer;

    [SerializeField] float timeToPlant, timeToGather;

    private DestroyedListener grainFieldListener;

    public override void ForceToWorkBuilding()
    {
        if (worker.IsState(Worker.WorkerState.WorkingOutside))
        {
            ExecuteExecutionOrder();
            return;
        }
    }

    protected override void Start()
    {
        base.Start();
        grainFields = new List<GrainFieldController>();
        environment = GameObject.Find("Environment").transform;
        gridManager = gameManager.GetComponent<GridManager>();
        grainFieldListener = GrainFieldDestroyed;
    }

    protected override void RemoveWorkerFinally()
    {
        base.RemoveWorkerFinally();
        init = true;
        waitTimer = 0;
        currentGrainField = null;
    }

    private void GrainFieldDestroyed(IDestroyable destroyedObject)
    {
        RemoveGrainField(destroyedObject as GrainFieldController);
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

            if (grainFields.Count == 0)
            {
                PlantGrainFieldExecution();
                return;
            }
            foreach (GrainFieldController grainField in grainFields)
            {
                if (grainField.IsReady() || grainField.IsDead())
                {
                    currentGrainField = grainField;
                    targetCell = currentGrainField.GetCell();
                    GatherGrainFieldExecution();
                    return;
                }
            }
            if (grainFields.Count != 2)
            {
                PlantGrainFieldExecution();
            }
        }
    }

    protected override void ExecutionFinished()
    {
        init = true;
        waitTimer = 0;
        worker.State = Worker.WorkerState.AtWork;
    }
    
    public void PlantGrainFieldExecution()
    {
        if (init)
        {
            worker.State = Worker.WorkerState.PlantGrainField;
            executionOrder.Enqueue(ChooseFreeCell);
            executionOrder.Enqueue(MoveToTargetCell);
            executionOrder.Enqueue(PlantGrainField);
            executionOrder.Enqueue(BackToWorkBuilding);
            nextWayPoint = -1;
            grainFieldPlantTimer = 0;
            init = false;
        }
    }

    public void GatherGrainFieldExecution()
    {
        if (init)
        {
            worker.State = Worker.WorkerState.GatherResource;
            executionOrder.Enqueue(MoveToTargetCell);
            executionOrder.Enqueue(GatherGrainField);
            executionOrder.Enqueue(BackToWorkBuilding);
            nextWayPoint = -1;
            grainFieldGatherTimer = 0;
            init = false;
        }
    }

    

    private bool ChooseFreeCell()
    {
        foreach (GridCell cell in gridManager.GetCellsAround(workPlace.GetCell()))
        {
            if (cell.IsFree())
            {
                targetCell = cell;
                targetCell.ReserveCell();
                return true;
            }
        }
        return false;
    }

    private void OnDestroy()
    {
        foreach(GrainFieldController grainField in grainFields)
        {
            grainField.RemoveDestroyedListener(grainFieldListener);
        }
    }

    private bool PlantGrainField()
    {
        if(grainFieldPlantTimer < timeToPlant)
        {
            grainFieldPlantTimer += Time.deltaTime;
            return false;
        }
        GrainFieldController grainField = GameObject.Instantiate(grainFieldPrefab, targetCell.centerPos, grainFieldPrefab.transform.rotation, environment);
        grainField.Init(targetCell);
        grainField.AddDestroyedListener(grainFieldListener);
        grainFields.Add(grainField);
        return true;
    }

    private void RemoveGrainField(GrainFieldController grainField)
    {
        grainFields.Remove(grainField);
    }

    private bool GatherGrainField()
    {
        if (grainFieldGatherTimer < timeToGather)
        {
            grainFieldGatherTimer += Time.deltaTime;
            return false;
        }

        if (currentGrainField.IsDead())
        {
            gatheredResources = 0;
        }
        else
        {
            gatheredResources = workBuilding.GetProduct().Amount;
        }
        currentGrainField.Destroy();
        return true;
    }

    
}
