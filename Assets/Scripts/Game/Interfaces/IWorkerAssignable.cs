using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorkerAssignable : IPlaceable
{
    WorkManager GetWorkManager(int index);
    bool IsLevelHighEnough(Worker worker);
}
