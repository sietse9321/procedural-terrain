using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDashable
{
    bool IsDashing { get; }
    void DashDirection(Vector3 direction);
}
