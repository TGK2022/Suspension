using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using UnityEngine.InputSystem;

public class TaskHoveredState : TaskBaseState
{
    public override void EnterState(TaskStateManager stateManager)
    {
        Debug.Log("Entered Hovered State");
        stateManager.OnTaskHoverEnter.Invoke();
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
                if (selectionRenderer != null && stateManager._playerStartTaskAction.WasPressedThisFrame())
                {
                    stateManager.SwitchState(stateManager.taskProgressingState);
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