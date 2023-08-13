using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayManager : MonoBehaviour
{
    private static GridManager gridManager;
    public void Start()
    {
        gridManager = GetComponent<GridManager>();
    }

    public static List<Vector3> FindWay(GridCell from, GridCell to)
    {
        List<Vector3> waypoints = new List<Vector3>();
        waypoints.Add(from.centerPos); //start pos
        waypoints.Add(from.GetLowerCenter()); //in front of the building
        GridCell wayEdge = gridManager.GetGridCell(to.posX, from.posZ);
        if (from.posX > to.posX)
        {
            waypoints.Add(wayEdge.GetLowerRightEdge());
            waypoints.Add(to.GetLowerRightEdge());
        }
        else
        {
            waypoints.Add(wayEdge.GetLowerLeftEdge());
            waypoints.Add(to.GetLowerLeftEdge());
        }
        waypoints.Add(to.GetLowerCenter()); //in front of target
        waypoints.Add(to.centerPos); //target pos
        return waypoints;
    }
}
