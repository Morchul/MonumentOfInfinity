using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour
{

    [SerializeField] float restTime, workTime, needInterval;
    [SerializeField] float moveSpeed;

    [SerializeField] int happiness;

    public Building LivingPlace { get; set; }
    private WorkManager workPlace;

    private float worktimer, timerUntilNeed;
    private bool isWorkTime;
    public WorkerState State { get { return state; } set { state = value; } }
    [SerializeField] WorkerState state; // only for debug

    private List<Vector3> way;
    private int nextWayPointIndex;

    [SerializeField] private  string workerName;
    [SerializeField] private int lvl;

    private GameObject gameManager;
    private ResourceManager resourceManager;
    private WorkerManager workerManager;

    public bool ForcedHomeNewWorkPlace { get; private set; }
    private WorkManager newWorkPlace;
    public bool NewWorkPlaceReady { private get; set; }

    Color lerpedColor = Color.white;

    private float strikeThreshold;

    public bool IsSoldier { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager");
        workerManager = gameManager.GetComponent<WorkerManager>();
        resourceManager = gameManager.GetComponent<ResourceManager>();
        worktimer = restTime -1;
        timerUntilNeed = 0;
        isWorkTime = false;
        State = WorkerState.AtHome;
        workerManager.AddWorker(this);
        ForcedHomeNewWorkPlace = false;
        strikeThreshold = StaticValues.GetStrikeThreshold();
    }

    
    // Update is called once per frame
    private void Update()
    {
        worktimer += Time.deltaTime;

        if (ForcedHomeNewWorkPlace) //new Workplace
        {

            if (IsState(WorkerState.AtHome)) //reached home now set new Workplace and set new Workmanager active
            {
                if (newWorkPlace == null || NewWorkPlaceReady)
                {
                    NewWorkPlaceReady = false;
                    workPlace = newWorkPlace;
                    newWorkPlace = null;
                    if (workPlace != null)
                    {
                        workPlace.SetActive();
                        way = WayManager.FindWay(LivingPlace.GetCell(), workPlace.GetWorkPlace().GetCell());
                    }
                    ForcedHomeNewWorkPlace = false;
                }
            }
            else if(IsState(WorkerState.AtWork) || IsState(WorkerState.WayToHome) || IsState(WorkerState.WayToWork))
            {
                GoHome();
            }
            return;
        }

        if (workPlace != null && isWorkTime && happiness > strikeThreshold && !LivingPlace.IsBurning() && !workPlace.IsBurning()) //work Time
        {
            if (IsState(WorkerState.AtWork) || IsState(WorkerState.WorkingOutside))
            {
                if (worktimer >= GetFinalWorkTime())
                {
                    isWorkTime = false;
                    return;
                }
            }
            else
            {
                GoToWork();
            }
        }
        else //rest time
        {
            if (IsState(WorkerState.AtHome))
            {
                if (LivingPlace.IsBurning())
                {
                    LivingPlace.ExtinguishFire(Time.deltaTime / 2);
                    return;
                }

                if (worktimer >= restTime && workPlace != null)
                {
                    isWorkTime = true;
                    return;
                }
                timerUntilNeed += Time.deltaTime;
                
                if(timerUntilNeed >= needInterval) {
                    ConsumeResources();
                    timerUntilNeed = 0;
                }
            }
            else
            {
                GoHome();
            }
        }
    }

    private void ConsumeResources()
    {
        foreach (Resource resource in LivingPlace.GetNeeds())
        {
            if (resourceManager.GetResource(resource))
            {
                if (happiness < 100)
                {
                    AddHappiness(1);
                }
            }
            else
            {
                AddHappiness(-resource.Amount * StaticValues.difficulty);
            }
        }
    }

    public bool IsWorkTime()
    {
        return isWorkTime;
    }

    public void AddHappiness(int happiness)
    {
        this.happiness += happiness;
        this.happiness = Mathf.Clamp(this.happiness, 0, 100);
        if (IsSoldier)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        } 
        else
        {
            lerpedColor = Color.Lerp(Color.red, Color.green, (float)this.happiness / 100);
            this.gameObject.GetComponent<Renderer>().material.color = lerpedColor;
        }
    }

    public int GetHappiness()
    {
        return happiness;
    }

    private float GetFinalWorkTime()
    {
        return workTime * happiness / 100;
    }

    public WorkManager GetWorkPlace()
    {
        return workPlace;
    }

    public void NewSoldierWorkPlace(WorkManager newSoldierWorkPlace)
    {
        if (workPlace != null)
            workPlace.RemoveWorker();
        this.newWorkPlace = newSoldierWorkPlace;
        ForcedHomeNewWorkPlace = true;
    }

    public void NewWorkPlace(WorkManager newWorkPlace)
    {

        if (IsSoldier)
        {
            ConvertFromSoldierToWorker();
            if (workPlace != null)
            {
                workPlace.RemoveWorker();
                way = WayManager.FindWay(LivingPlace.GetCell(), workPlace.GetWorkPlace().GetCell());
            }
            else
            {
                way = WayManager.FindWay(LivingPlace.GetCell(), barrackBuilding.GetCell());
            }
            barrackWorkManager.RemoveWorker();
        }
        else
        {
            if (workPlace != null)
                workPlace.RemoveWorker();
        }
        this.newWorkPlace = newWorkPlace;
        ForcedHomeNewWorkPlace = true;
    }

    private void OnDestroy()
    {
        if(workPlace != null)
            workPlace.RemoveWorker(true);
        workerManager.RemoveWorker(this);
    }

    public int GetLevel()
    {
        return lvl;
    }

    public string GetName()
    {
        return workerName;
    }

    public bool HasWork()
    {
        return newWorkPlace != null || workPlace != null;
    }

    private Building originLivingPlace;
    private float originNeedInterval;

    private Building barrackBuilding;
    private WorkManager barrackWorkManager;

    public void ConvertToSoldier(Building barrack, WorkManager barrackWorkManager)
    {
        IsSoldier = true;
        barrackBuilding = barrack;
        this.barrackWorkManager = barrackWorkManager;

        originLivingPlace = LivingPlace;
        originNeedInterval = needInterval;

        needInterval = 10; // soldier interval
        LivingPlace = barrack;
        workPlace = null;

        way = WayManager.FindWay(LivingPlace.GetCell(), originLivingPlace.GetCell());


        AddHappiness(0); //To change the color
        State = WorkerState.AtWork;
        NewSoldierWorkPlace(null);
    }

    private void ConvertFromSoldierToWorker()
    {
        IsSoldier = false;
        needInterval = originNeedInterval;
        LivingPlace = originLivingPlace;
        AddHappiness(0); //To change the color
        State = WorkerState.AtWork;
    }

    void GoHome()
    {
        if (IsState(WorkerState.AtHome) || IsState(WorkerState.WorkingOutside)) return;
        if (IsState(WorkerState.AtWork))
        {
            State = WorkerState.WayToHome;
            nextWayPointIndex = way.Count - 1;
        }

        Vector3 directionVec = way[nextWayPointIndex] - gameObject.transform.position;
        if ((directionVec).magnitude < 0.1f)
        {
            //reached
            if (--nextWayPointIndex < 0)
            {
                State = WorkerState.AtHome;
                worktimer = 0;
                return;
            }
        }
        //walk
        Move(directionVec);
    }

    void GoToWork()
    {
        if (IsState(WorkerState.AtWork)) return;
        if (IsState(WorkerState.AtHome))
        {
            State = WorkerState.WayToWork;
            nextWayPointIndex = 0;
        }

        Vector3 directionVec = way[nextWayPointIndex] - gameObject.transform.position;
        if ((directionVec).magnitude < 0.1f)
        {
            //reached
            if (++nextWayPointIndex == way.Count)
            {
                State = WorkerState.AtWork;
                worktimer = 0;
                return;
            }
        }
        //walk
        Move(directionVec);
    }

    public void Move(Vector3 directionVec)
    {
        gameObject.transform.Translate(directionVec.normalized * moveSpeed * Time.deltaTime);
    }

    public bool IsState(WorkerState state)
    {
        return (this.State & state) == state;
    }

    [System.Serializable]
    public enum WorkerState
    {
        AtHome = 0x01,
        AtWork = 0x02,
        WayToWork = 0x04,
        WayToHome = 0x08,

        WorkingOutside = 0x10,
        PlantGrainField = 0x30, //WorkingOutside + plantGrainField
        GatherResource = 0x50 //WorkingOutside + gatherResource
    }
}
