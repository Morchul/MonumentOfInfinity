using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    [SerializeField] List<Event> events;
    [SerializeField] float delay;
    [SerializeField] float eventTestTime;
    [SerializeField] float averageEventInterval;

    private UIManager uiManager;

    private float timer;
    private float eventChanceTimer;
    private bool eventsHappening;
    private float eventTestTimer;

    // Start is called before the first frame update
    void Start()
    {
        eventsHappening = false;
        uiManager = GameController.FindGameManager().GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > delay && !eventsHappening)
        {
            eventsHappening = true;
            timer = 0;
        }

        if (eventsHappening)
        {
            eventChanceTimer += Time.deltaTime;
            eventTestTimer += Time.deltaTime;
            if (eventTestTimer > eventTestTime)
            {
                eventTestTimer = 0;
                if (GameController.PercentChance(GetEventChance()))
                {
                    int eventTries = 0;
                    while (eventTries < 6)
                    {
                        Event selectedEvent = events[Random.Range(0, events.Count)];
                        if (selectedEvent.CanTrigger(timer))
                        {
                            
                            selectedEvent.Trigger();
                            uiManager.ShowMessage(selectedEvent.GetName() + " has started");
                            eventChanceTimer = 0;

                            break;
                        }
                        ++eventTries;
                    }
                }
            }
        }
    }

    private float GetEventChance()
    {
        if (eventChanceTimer < averageEventInterval) return 0;
        float chance = eventChanceTimer / 3 + (eventChanceTimer - averageEventInterval);
        //float chance = eventChanceTimer / 3 + (eventChanceTimer / 7); //only for test
        return chance;
    }
}
