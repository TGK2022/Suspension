using UnityEngine.InputSystem;
using UnityEngine;

public class TaskInactiveState : TaskBaseState
{
    public override void EnterState(TaskStateManager stateManager)
    {
        Debug.Log("Entered Inactive State");
        stateManager.OnTaskHoverExit.Invoke();
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
                if (selectionRenderer != null)
                {
                    stateManager.SwitchState(stateManager.taskHoveredState);
                }
            }
        }
    }
}