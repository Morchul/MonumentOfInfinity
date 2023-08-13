using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : EnemyWorkflow
{

    private Vector3 startPos;

    protected override void Start()
    {
        base.Start();
        startPos = gameObject.transform.position;
    }

    protected override void RestFinished() { }

    protected override void ReachedTarget() { }

    protected override void WorkFinished()
    {
        if (target == null) return;
        if (GameController.PercentChance(40))
        {
            SetTargetOnFire();
        }
    }

    protected override void ReachedHome()
    {
        if (target != null)
            StealResources();
    }

    private bool isHomeCell;

    protected override bool MoveToHome()
    {
        if (IsState(Worker.WorkerState.AtHome) || IsState(Worker.WorkerState.WorkingOutside)) return true;

        Vector3 directionVec;
        if (isHomeCell)
        {
            directionVec = startPos - transform.position;
            if(directionVec.magnitude < 0.1f)
            {
                isHomeCell = false;
                return true;
            }
            else
            {
                Move(directionVec);
                return false;
            }
                
        }
        directionVec = way[nextWayPointIndex] - gameObject.transform.position;
        if ((directionVec).magnitude < 0.1f)
        {
            //reached
            if (--nextWayPointIndex < 0)
            {
                isHomeCell = true;
            }
        }
        //walk
        Move(directionVec);
        return false;
    }
}
