using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBurnable : IDestroyable
{
    bool IsBurnable();
    bool IsBurning();
    void SetOnFire();
    void FireExtinguished();
}
