using System.Collections.Generic;
using UnityEngine;

public abstract class SoldierAssignableEvent : PlaceableEvent, IWorkerAssignable, IDestroyable
{

    private WorkManager[] workManagers;
    [SerializeField] WorkManager workManager;
    [SerializeField] int maxSoldiers;
    private static GameObject workManagersParent;

    private List<DestroyedListener> destroyedListeners;
    private bool destroyed;

    [SerializeField] protected float hp;


    private void Awake()
    {
        destroyedListeners = new List<DestroyedListener>();
    }
    public virtual void Start()
    {
        if (workManagersParent == null)
            workManagersParent = GameController.FindWorkManagers();
        workManagers = new WorkManager[maxSoldiers];

        for (int i = 0; i < maxSoldiers; ++i)
        {
            workManagers[i] = Instantiate(workManager, workManagersParent.transform);
            workManagers[i].Init(this);
        }
        destroyed = false;
        switch (StaticValues.difficulty)
        {
            case 2: hp += 3; break;
            case 3: hp += 6; break;
        }
    }

    public virtual void Update()
    {
        if(!destroyed && hp <= 0)
        {
            Destroy(gameObject);
            destroyed = true;
        }
    }

    public void SoldierWork(float time)
    {
        hp -= time;
    }

    public WorkManager GetWorkManager(int index)
    {
        return workManagers[index];
    }

    public bool IsLevelHighEnough(Worker worker)
    {
        return true;
    }

    protected virtual void OnDestroy()
    {
        foreach(DestroyedListener destroyedListener in destroyedListeners)
            destroyedListener?.Invoke(this);
        destroyedListeners.Clear();
        foreach (WorkManager workManager in workManagers)
        {
            workManager.SetDestroy();
        }
        GetCell().RemoveEventObject();
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public void Repair(float hpRepaired)
    {
        //Can't repair events
    }

    public void Destroy()
    {
        hp = 0;
    }

    public bool AddDestroyedListener(DestroyedListener listener)
    {
        if(hp > 0)
        {
            destroyedListeners.Add(listener);
            return true;
        }
        return false;
        
    }

    public void RemoveDestroyedListener(DestroyedListener listener)
    {
        destroyedListeners.Remove(listener);
    }
}
