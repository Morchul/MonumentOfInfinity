using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Profiling;
using UnityEngine;

public class BillingSystem : MonoBehaviour
{
    [SerializeField] float billingTime;
    [SerializeField] int billingAmount;
    float billingTimer;

    ResourceManager resourceManager;
    WorkerManager workerManager;
    // Start is called before the first frame update
    void Start()
    {
        billingTimer = 0;
        resourceManager = GetComponent<ResourceManager>();
        workerManager = GetComponent<WorkerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        billingTimer += Time.deltaTime;   

        if(billingTimer >= billingTime)
        {
            resourceManager.AddResource(ResourceType.Money, GetBillingAmount());
            billingTimer = 0;
        }
    }

    private int lastWorkerCount;
    private int billing;

    public int GetBillingAmount()
    {
        if (lastWorkerCount != workerManager.GetWorkerCount())
        {
            lastWorkerCount = workerManager.GetWorkerCount();
            billing = 0;
            foreach (Worker worker in workerManager.GetWorkers())
            {
                if (!worker.IsSoldier)
                {
                    billing += (int)(billingAmount * ((float)worker.GetHappiness() / 100));
                }
            }
        }
        return billing;
    }

    public int GetTimeUntilBilling()
    {
        return (int)(billingTime - billingTimer);
    }
}
