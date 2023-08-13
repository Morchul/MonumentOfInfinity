using UnityEngine;

public class SoldierWorkManager : WorkManager
{

    SoldierAssignableEvent soldierAssignableEvent;

    public override void ForceToWorkBuilding()
    {
        
    }

    public override void Work()
    {
        if (soldierAssignableEvent == null) soldierAssignableEvent = workPlace as SoldierAssignableEvent;

        soldierAssignableEvent.SoldierWork(Time.deltaTime);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void FireWorker()
    {
        if (worker != null)
            worker.NewSoldierWorkPlace(null);
    }

    protected override void RemoveWorkerFinally()
    {
        soldierAssignableEvent = null;
        base.RemoveWorkerFinally();
    }
}
