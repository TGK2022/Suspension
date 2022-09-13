using System.Diagnostics;

public class TaskDoneState : TaskBaseState
{
    public override void EnterState(TaskStateManager stateManager)
    {
        Debug.Print("Entered Done State");
        stateManager.OnTaskDone.Invoke();
    }

    public override void UpdateState(TaskStateManager stateManager)
    {
    }
}