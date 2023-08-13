using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    GridCell GetCell();
    GameObject GetGameObject();
    void Init(GridCell cell);
}
