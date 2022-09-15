using UnityEngine;
using UnityEngine.InputSystem;

public class TaskProgressingState : TaskBaseState
{
    public override void EnterState(TaskStateManager stateManager)
    {
        Debug.Log("Entered Progressing State");
        stateManager.OnTaskProgressing.Invoke();
    }

    public override void UpdateState(TaskStateManager stateManager)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, stateManager.maxDistanceToTask))
        {
            Transform selection = hit.transform;
            if (selection.CompareTag(stateManager.taskTag))
            {
                Renderer selectionRenderer = selection.GetComponent<Renderer>();
                // Should activate when Hold button is performed
                if (selectionRenderer != null && stateManager._playerStartTaskAction.WasPerformedThisFrame())
                {
                    stateManager.SwitchState(stateManager.taskDoneState);
                }
                // If player stopped pressing button should return to HoveredState
                else if (selectionRenderer != null && stateManager._playerStartTaskAction.WasReleasedThisFrame())
                {
                    stateManager.SwitchState(stateManager.taskHoveredState);
                }
            }
            else
            {
                stateManager.SwitchState(stateManager.taskInactiveState);
            }
        }
        else
        {
            stateManager.SwitchState(stateManager.taskInactiveState);
        }
    }
}