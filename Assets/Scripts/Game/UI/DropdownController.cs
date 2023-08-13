using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownController : MonoBehaviour
{

    Dropdown dropdown;
    private List<DropdownListener> listeners = new List<DropdownListener>();
    [SerializeField] WorkerManager workerManager;
    [SerializeField] int workerIndex;
    [SerializeField] bool soldier;

    private IWorkerAssignable workerAssignable;

    private void Awake()
    {
        //listeners = new List<DropdownListener>();
        dropdown = GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate { DropDownValueChanged(dropdown); });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Soldier()
    {
        return soldier;
    }

    private void DropDownValueChanged(Dropdown dropdown)
    {
        if (dropdown.value == dropdown.options.Count -1)
        {
            return;
        }
        ValueChanged(dropdown.value);
        UpdateDropdownValues();
    }


    public void SelectedWorkerAssignableChanged(IWorkerAssignable workerAssignable)
    {
        this.workerAssignable = workerAssignable;
    }

    public void UpdateDropdownValues()
    {
        dropdown.options.Clear();

        dropdown.options.Add(new Dropdown.OptionData("")); // Add null
        List<Worker> unemployedWorker = soldier ? workerManager.GetUnemployedSoldiers() : workerManager.GetUnemployedWorker();
        foreach (Worker worker in unemployedWorker) // Add unempoloyed worker or soldier
        {
            dropdown.options.Add(new Dropdown.OptionData(worker.GetName() + "(" + worker.GetLevel() + ")"));
        }
        if (workerAssignable != null)
        {
            Worker selectedWorker = workerAssignable.GetWorkManager(workerIndex).GetWorker(); // Add selected worker
            dropdown.options.Add(new Dropdown.OptionData(selectedWorker == null ? "" : selectedWorker.GetName() + "(" + selectedWorker.GetLevel() + ")"));
        }
        /*else
        {
            dropdown.options.Add(new Dropdown.OptionData(""));
        }*/

        dropdown.value = dropdown.options.Count - 1;
        dropdown.RefreshShownValue();
    }

    private void ValueChanged(int value)
    {
        foreach(DropdownListener listener in listeners)
        {
            listener(value, workerIndex);
        }
        dropdown.Hide();
    }

    public void AddListener(DropdownListener listener)
    {
        listeners.Add(listener);
    }

    public delegate void DropdownListener(int index, int workerIndex);
}
