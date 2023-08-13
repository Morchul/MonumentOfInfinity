using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkBuilding : Building, IWorkerAssignable
{

    private WorkManager[] workManagers;
    [SerializeField] float gatherInterval;
    [SerializeField] Resource product;
    [SerializeField] WorkManager workManager;
    private static GameObject workManagersParent;

    // Start is called before the first frame update
    void Start()
    {
        if (workManagersParent == null)
            workManagersParent = GameController.FindWorkManagers();
        workManagers = new WorkManager[maxWorker];

        for (int i = 0; i < maxWorker; ++i)
        {

            workManagers[i] = Instantiate(workManager, workManagersParent.transform);
            workManagers[i].Init(this);
        }
    }

    public float GetGatherInteval()
    {
        return gatherInterval;
    }

    public Resource GetProduct()
    {
        return product;
    }

    public WorkManager GetWorkManager(int index)
    {
        return workManagers[index];
    }

    public bool IsLevelHighEnough(Worker worker)
    {
        return worker.GetLevel() >= GetLevel();
    }

    protected override void OnDestroy()
    {
        foreach(WorkManager workManager in workManagers)
        {
            workManager.SetDestroy();
        }
        base.OnDestroy();
    }
}
