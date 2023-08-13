using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class WorkManager : MonoBehaviour
{

    protected Worker worker;
    protected Worker waitingWorker;
    protected IWorkerAssignable workPlace;

    protected GameObject gameManager;
    protected ResourceManager resourceManager;

    private bool active, removeWorker;
    protected bool destroy;

    public virtual void Init(IWorkerAssignable workPlace)
    {
        removeWorker = false;
        this.workPlace = workPlace;
        gameManager = GameController.FindGameManager();
        resourceManager = gameManager.GetComponent<ResourceManager>();
        destroy = false;
    }

    // Update is called once per frame
    public void Update()
    {
        if (worker == null || workPlace == null) return;

        if (removeWorker || destroy)
        {
            if (worker.IsState(Worker.WorkerState.WorkingOutside))
                ForceToWorkBuilding();
            else
            {
                RemoveWorkerFinally();
            }
            return;
        }

        if (!active) return;
        if (worker.IsWorkTime())
        {
            if (worker.IsState(Worker.WorkerState.AtWork) || worker.IsState(Worker.WorkerState.WorkingOutside))
            {
                Work();
            }
        }
        else
        {
            if (worker.IsState(Worker.WorkerState.WorkingOutside))
            {
                ForceToWorkBuilding();
            }
        }
    }

    public bool IsBurning()
    {
        if(workPlace is IBurnable)
        {
            return (workPlace as IBurnable).IsBurning();
        }
        return false;
    }

    public void SetActive()
    {
        active = true;
    }

    public bool IsFree()
    {
        return this.worker == null;
    }

    public virtual void AssignWorker(Worker worker)
    {
        if (IsFree())
        {
            this.worker = worker;
            if(worker != null)
                this.worker.NewWorkPlaceReady = true;
        }
        else
            waitingWorker = worker;
    }

    protected virtual void RemoveWorkerFinally()
    {
        if (destroy)
        {
            FireWorker();
        }
        this.worker = null;
        active = false;
        removeWorker = false;
        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            if(waitingWorker != null)
            {
                AssignWorker(waitingWorker);
                waitingWorker.NewWorkPlaceReady = true;
                waitingWorker = null;
            }
        }
    }

    public virtual void RemoveWorker(bool instantly = false)
    {
        removeWorker = true;
        if (instantly) RemoveWorkerFinally();
    }

    public virtual void FireWorker()
    {
        if(worker != null)
            worker.NewWorkPlace(null);
    }

    public Worker GetWorker()
    {
        if (removeWorker)
        {
            if (waitingWorker != null)
                return waitingWorker;
            return null;
        }
        return worker;
    }

    public IWorkerAssignable GetWorkPlace()
    {
        return workPlace;
    }

    public void SetDestroy()
    {
        destroy = true;
    }

    /*private void OnDestroy()
    {
        FireWorker();
    }*/

    public abstract void Work();
    public abstract void ForceToWorkBuilding();
}
