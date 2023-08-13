using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyWorkflow : MonoBehaviour
{

    [SerializeField] float restTime, workTime;
    [SerializeField] float moveSpeed;

    [SerializeField] Fire FirePrefab;

    protected float restTimer, workTimer;

    protected GameObject gameManager;

    protected Worker.WorkerState state;

    protected int nextWayPointIndex;
    protected List<Vector3> way;

    protected Building target;
    protected SoldierAssignableEvent home;


    [SerializeField] int stolenResourcesAmount;
    [SerializeField] int stolenMoneyAmount;

    protected ResourceManager resourceManager;
    protected Resource stolenResource;
    protected BuildingManager buildingManager;

    private DestroyedListener destroyedListener;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        home = GetComponentInParent<SoldierAssignableEvent>();
        gameManager = GameController.FindGameManager();
        buildingManager = gameManager.GetComponent<BuildingManager>();
        resourceManager = gameManager.GetComponent<ResourceManager>();
        state = Worker.WorkerState.AtHome;
        destroyedListener = RaidTargetDestroyed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (home.GetCell() == null) return;
        if(target == null)
        {
            if (IsState(Worker.WorkerState.AtHome))
            {
                FindNewTarget();
                return;
            }
        }

        if (state == Worker.WorkerState.AtHome)
        {
            restTimer += Time.deltaTime;
            if (restTimer >= restTime)
            {
                restTimer = 0;
                state = Worker.WorkerState.WayToWork;
                nextWayPointIndex = 0;
                RestFinished();
            }

        }
        else if (state == Worker.WorkerState.WayToWork)
        {
            if (MoveToWork())
            {
                if (target != null)
                {
                    state = Worker.WorkerState.AtWork;
                    ReachedTarget();
                }
                else
                {
                    state = Worker.WorkerState.WayToHome;
                }
            }
        }
        else if (state == Worker.WorkerState.AtWork)
        {
            workTimer += Time.deltaTime;
            if (target == null || workTimer >= workTime)
            {
                workTimer = 0;
                state = Worker.WorkerState.WayToHome;
                nextWayPointIndex = way.Count - 1;
                WorkFinished();
            }
        }
        else if (state == Worker.WorkerState.WayToHome)
        {
            if (MoveToHome())
            {
                state = Worker.WorkerState.AtHome;
                ReachedHome();
            }
        }
    }

    private void OnDestroy()
    {
        if(target != null)
        {
            target.RemoveDestroyedListener(destroyedListener);
        }
    }

    private void RaidTargetDestroyed(IDestroyable destroyedTarget)
    {
        target = null;
    }

    protected void FindNewTarget()
    {
        List<Building> raidableBuildings = buildingManager.GetAllRaidableBuildings();
        if (raidableBuildings.Count == 0)
        {
            return;
        }

        target = raidableBuildings[Random.Range(0, raidableBuildings.Count)];
        target.AddDestroyedListener(destroyedListener);
        way = WayManager.FindWay(home.GetCell(), target.GetCell());
        if (target is WorkBuilding)
        {
            stolenResource = new Resource((target as WorkBuilding).GetProduct().Type, stolenResourcesAmount);
        }
        else if (target is House)
        {
            stolenResource = new Resource(ResourceType.Money, stolenMoneyAmount);

        }
        else
        {
            Debug.LogError("Can only steal from WorkBuilding or House");
        }
    }

    protected abstract void RestFinished();
    protected abstract void ReachedTarget();
    protected abstract void WorkFinished();
    protected abstract void ReachedHome();

    protected void SetTargetOnFire()
    {
        if (target.IsBurnable())
        {
            Fire fire = Instantiate(FirePrefab, target.GetCell().centerPos, Quaternion.Euler(-90, 0, 0));
            fire.Init(target.GetCell());
            target = null;
        }
    }

    protected void StealResources()
    {
        if (!resourceManager.GetResource(stolenResource))
        {
            resourceManager.GetResource(stolenResource.Type, resourceManager.GetResource(stolenResource.Type));
        }
    }

    protected virtual bool MoveToHome()
    {
        if (IsState(Worker.WorkerState.AtHome) || IsState(Worker.WorkerState.WorkingOutside)) return true;

        Vector3 directionVec = way[nextWayPointIndex] - gameObject.transform.position;
        if ((directionVec).magnitude < 0.1f)
        {
            //reached
            if (--nextWayPointIndex < 0)
            {
                return true;
            }
        }
        //walk
        Move(directionVec);
        return false;
    }

    protected bool MoveToWork()
    {
        if (IsState(Worker.WorkerState.AtWork)) return true;

        Vector3 directionVec = way[nextWayPointIndex] - gameObject.transform.position;
        if ((directionVec).magnitude < 0.1f)
        {
            //reached
            if (++nextWayPointIndex == way.Count)
            {
                return true;
            }
        }
        //walk
        Move(directionVec);
        return false;
    }

    protected bool IsState(Worker.WorkerState state)
    {
        return (this.state & state) == state;
    }

    protected void Move(Vector3 directionVec)
    {
        gameObject.transform.Translate(directionVec.normalized * moveSpeed * Time.deltaTime);
    }
}
