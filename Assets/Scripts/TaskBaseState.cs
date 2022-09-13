using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TaskBaseState
{
    public abstract void EnterState(TaskStateManager state);
    public abstract void UpdateState(TaskStateManager state);
}
