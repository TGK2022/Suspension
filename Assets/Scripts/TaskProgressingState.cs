using UnityEngine;
using UnityEngine.InputSystem;

public class TaskProgressingState : TaskBaseState
{
    private float startTime = 0.0f;

    public override void EnterState(TaskStateManager stateManager)
    {
        Debug.Log("Entered Progressing State");
        startTime = Time.time;
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
                
                // If player stopped pressing button should return to HoveredState
                if (selectionRenderer != null && stateManager._playerStartTaskAction.WasReleasedThisFrame())
                {
                    stateManager.SwitchState(stateManager.taskHoveredState);
                }
                // Should activate when Hold button is performed and time has passed
                else if (selectionRenderer != null && (Time.time - startTime) > stateManager.timeToFinish)
                {
                    stateManager.SwitchState(stateManager.taskDoneState);
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