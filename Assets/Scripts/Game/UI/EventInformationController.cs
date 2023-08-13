using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventInformationController : MonoBehaviour
{
    [SerializeField] Text eventName;
    [SerializeField] Text eventInformation;

    [SerializeField] DropdownController firstSoldierChooser;
    [SerializeField] DropdownController secondSoldierChooser;

    Event currentEvent;

    WorkerManager workerManager;

    private DestroyedListener listener;

    // Start is called before the first frame update
    void Start()
    {
        firstSoldierChooser.AddListener(SoldierChooserChanged);
        secondSoldierChooser.AddListener(SoldierChooserChanged);
        workerManager = GameController.FindGameManager().GetComponent<WorkerManager>();
        listener = EventDestroyed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EventDestroyed(IDestroyable _)
    {
        HideEventInformation();
    }

    public void ShowEventInformation(Event _event)
    {

        if(currentEvent != null && currentEvent is SoldierAssignableEvent)
        {
            (currentEvent as SoldierAssignableEvent).RemoveDestroyedListener(listener);
        }

        ShowEventInformation();
        currentEvent = _event;

        if(currentEvent != null)
        {
            eventName.text = currentEvent.GetName();
            eventInformation.text = currentEvent.GetEventInformation();

            if(currentEvent is SoldierAssignableEvent)
            {
                SoldierAssignableEvent soldierAssignableEvent = currentEvent as SoldierAssignableEvent;
                firstSoldierChooser.gameObject.SetActive(true);
                secondSoldierChooser.gameObject.SetActive(true);
                firstSoldierChooser.SelectedWorkerAssignableChanged(soldierAssignableEvent);
                secondSoldierChooser.SelectedWorkerAssignableChanged(soldierAssignableEvent);
                soldierAssignableEvent.AddDestroyedListener(listener);

                UpdateSoldierChooser();
            }
            else
            {
                firstSoldierChooser.gameObject.SetActive(false);
                secondSoldierChooser.gameObject.SetActive(false);
            }
        }
        
    }

    public void SoldierChooserChanged(int value, int workerIndex)
    {
        Worker worker;
        IWorkerAssignable workerAssignable = currentEvent as IWorkerAssignable;

        if (value == 0) // no worker selected
        {
            //worker = null;
            Worker currentWorker = workerAssignable.GetWorkManager(workerIndex).GetWorker();
            if (currentWorker != null)
                currentWorker.NewSoldierWorkPlace(null);
        }
        else
        {
            worker = workerManager.GetUnemployedSoldiers()[value - 1];
            if (workerAssignable.IsLevelHighEnough(worker))
            {
                WorkManager workManager = workerAssignable.GetWorkManager(workerIndex);
                Worker currentWorker = workManager.GetWorker();
                if (currentWorker != null)
                    currentWorker.NewSoldierWorkPlace(null);

                worker.NewSoldierWorkPlace(workManager); //assign new Workplace but still inactive
                workManager.AssignWorker(worker); //assign to Workmanager but still inactive
            }
        }
        //workBuilding.SetWorker(workerIndex, worker);
        UpdateSoldierChooser();
    }

    public void UpdateSoldierChooser()
    {
        firstSoldierChooser.UpdateDropdownValues();
        secondSoldierChooser.UpdateDropdownValues();
    }

    public void ShowEventInformation()
    {
        gameObject.SetActive(true);
    }

    public void HideEventInformation()
    {
        gameObject.SetActive(false);
    }
}
