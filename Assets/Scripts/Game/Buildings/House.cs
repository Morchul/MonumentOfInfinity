using System.Collections.Generic;
using UnityEngine;

public class House : Building
{

    [SerializeField] protected Worker workerPrefab;
    private List<Worker> workers;

    // Start is called before the first frame update
    private void Awake()
    {
        workers = new List<Worker>(maxWorker);
    }

    public override void Init(GridCell gridCell)
    {
        base.Init(gridCell);

        for(int i = 0; i < maxWorker; ++i)
        {
            Worker worker = Instantiate(workerPrefab, gridCell.centerPos + new Vector3(0,0.5f,0), Quaternion.identity, transform);
            workers.Add(worker);
            Worker workerComponent = worker.GetComponent<Worker>();
            workerComponent.LivingPlace = this;
        }
    }

    protected override void OnDestroy()
    {
        foreach (Worker worker in workers)
        {
            if(worker != null)
                Destroy(worker.gameObject);
        }
        base.OnDestroy();
    }
}
