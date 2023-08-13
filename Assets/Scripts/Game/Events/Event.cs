using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Event : MonoBehaviour
{
    [SerializeField] protected float delay;
    [SerializeField] protected string eventName;
    [SerializeField] protected string eventInformation;

    public string GetName()
    {
        return eventName;
    }

    public string GetEventInformation()
    {
        return eventInformation;
    }

    public virtual bool CanTrigger(float timer)
    {
        if (timer < ((delay / StaticValues.difficulty) + (StaticValues.difficulty * 10))) return false;
        return true;
    }

    public abstract void Trigger();
}
