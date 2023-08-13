using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{

    [SerializeField] Text workerDisplay;
    [SerializeField] Text soldierDisplay;
    [SerializeField] Text happiness;
    [SerializeField] Text age;

    [SerializeField] WorkerManager workerManager;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        workerDisplay.text = "Workers: " + workerManager.GetUnemployedWorker().Count + "/" + workerManager.GetWorkerCount();
        soldierDisplay.text = "Soldiers: " + workerManager.GetUnemployedSoldiers().Count;
        happiness.text = "Happiness: " + workerManager.GetAverageHappiness() + "/100";

        age.text = "Age: " + (20 + (int)(GameController.AgeTimer / 60));
    }
}
