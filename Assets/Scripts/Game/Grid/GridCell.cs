using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public readonly float gridCellWidth;
    public readonly int posX, posZ;
    public readonly Vector3 centerPos;
    private bool free;

    private IPlaceable cellObject;
    private PlaceableEvent eventCellObject;

    public GridCell(float gridCellWidth, Vector3 centerPos, int posX, int posZ)
    {
        this.gridCellWidth = gridCellWidth;
        this.centerPos = centerPos;
        this.free = true;
        this.posX = posX;
        this.posZ = posZ;
    }

    public Vector3 GetLowerLeftEdge()
    {
        return new Vector3(centerPos.x - gridCellWidth / 2,0, centerPos.z - gridCellWidth / 2);
    }

    public Vector3 GetLowerRightEdge()
    {
        return new Vector3(centerPos.x + gridCellWidth / 2, 0, centerPos.z - gridCellWidth / 2);
    }

    public Vector3 GetLowerCenter()
    {
        return new Vector3(centerPos.x, 0, centerPos.z - gridCellWidth / 2);
    }

    public bool IsFree()
    {
        return free;
    }

    public IPlaceable GetPlacedObject()
    {
        return cellObject;
    }

    public void PlaceObject(IPlaceable cellObject)
    {
        free = false;
        this.cellObject = cellObject;
    }

    public void ReserveCell()
    {
        free = false;
    }

    public void RemoveObject()
    {
        cellObject = null;
        free = true;
    }

    public PlaceableEvent GetPlacedEvent()
    {
        return eventCellObject;
    }

    public void PlaceEventObject(PlaceableEvent eventCellObject)
    {
        if (eventCellObject.Block)
        {
            free = false;
        }
        this.eventCellObject = eventCellObject;
    }

    public void RemoveEventObject()
    {
        if(eventCellObject != null)
            if (eventCellObject.Block)
                free = true;
        this.eventCellObject = null;
    }

    public bool HasEventObject()
    {
        return eventCellObject != null;
    }

    
}
