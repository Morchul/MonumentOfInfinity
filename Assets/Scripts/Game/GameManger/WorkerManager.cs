using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WorkerManager : MonoBehaviour
{
    private List<Worker> workers;

    private void Awake()
    {
        workers = new List<Worker>();
    }

    public void AddWorker(Worker worker)
    {
        workers.Add(worker);
    }

    public void RemoveWorker(Worker worker)
    {
        workers.Remove(worker);
    }

    public Worker GetWorker(int index)
    {
        return workers[index];
    }

    public List<Worker> GetWorkers()
    {
        return workers;
    }

    public int GetWorkerCount()
    {
        int count = 0;
        foreach(Worker worker in workers)
        {
            if (!worker.IsSoldier) ++count;
        }
        return count;
    }

    public int GetAverageHappiness()
    {
        if (workers.Count == 0) return 100;
        int totalHappiness = 0;
        foreach(Worker worker in workers)
        {
            totalHappiness += worker.GetHappiness();
        }
        return totalHappiness / workers.Count;
    }

    public List<Worker> GetUnemployedWorker()
    {
        List<Worker> unemployedWorkers = new List<Worker>();
        foreach(Worker worker in workers)
        {
            if(!worker.IsSoldier && !worker.HasWork())
            {
                unemployedWorkers.Add(worker);
            }
        }
        return unemployedWorkers;
    }

    public List<Worker> GetUnemployedSoldiers()
    {
        List<Worker> soldiers = new List<Worker>();
        foreach(Worker worker in workers)
        {
            if(worker.IsSoldier && !worker.HasWork())
            {
                soldiers.Add(worker);
            }
        }
        return soldiers;
    }
}
