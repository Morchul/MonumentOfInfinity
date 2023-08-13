using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DroughtEvent : Event
{

    private ResourceManager resourceManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //No additional CanTrigger conditions

    public override void Trigger()
    {
        if(resourceManager == null)
            resourceManager = GameController.FindGameManager().GetComponent<ResourceManager>();
        resourceManager.GetResource(ResourceType.Grain, resourceManager.GetResource(ResourceType.Grain) / 3);
        GrainFieldController[] grainFields = GameObject.FindObjectsOfType<GrainFieldController>();
        foreach(GrainFieldController grainFieldController in grainFields)
        {
            grainFieldController.Drought();
        }
    }
}
