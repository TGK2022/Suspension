using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TaskStateManager : MonoBehaviour
{
    TaskBaseState currentState = null;
    public TaskInactiveState taskInactiveState = new();
    public TaskHoveredState taskHoveredState = new();
    public TaskProgressingState taskProgressingState = new();
    public TaskDoneState taskDoneState = new();

    public string taskTag = "Task";
    public float maxDistanceToTask = 1f;
    public InputActionAsset inputActions = null;
    public InputActionMap _playerTasksActionMap;
    public InputAction _playerStartTaskAction;

    public UnityEvent OnTaskHoverEnter;
    public UnityEvent OnTaskHoverExit;
    public UnityEvent OnTaskDone;
    public UnityEvent OnTaskProgressing;

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }

    protected void Awake()
    {
        if (inputActions is null)
        {
            Debug.LogError("TaskInputAction requires inputActions to be assigned!");
            return;
        }

        _playerTasksActionMap = inputActions.FindActionMap("PlayerTasks", throwIfNotFound: true);
        _playerStartTaskAction = _playerTasksActionMap.FindAction("StartTask", throwIfNotFound: true);
    }

    void Start()
    {
        currentState = taskInactiveState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(TaskBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void StartTaskOnDemand()
    {
        currentState = taskProgressingState;
        currentState.EnterState(this);
    }

    public void StopTaskOnDemand()
    {
        currentState = taskInactiveState;
        currentState.EnterState(this);
    }

}
