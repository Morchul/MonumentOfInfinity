using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceableEvent : Event, IPlaceable
{
    protected GridCell cell;
    public bool Block { get; protected set; }

    public GridCell GetCell()
    {
        return cell;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public virtual void Init(GridCell cell)
    {
        this.cell = cell;
        cell.PlaceEventObject(this);
    }
}
