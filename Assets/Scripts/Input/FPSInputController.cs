using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

public class FPSInputController
    : AbstractCharacterInputController
{
    public InputActionAsset inputActions = null;
    public bool smoothLooking = true;
    [EnableIf(nameof(smoothLooking))]
    [AllowNesting]
    public float lookSmoothingFactor = 14.0f;

    public override IObservable<Vector2> Move => _move;
    private IObservable<Vector2> _move;

    public override IObservable<Unit> Jump => _jump;
    private Subject<Unit> _jump;

    public override ReadOnlyReactiveProperty<bool> Run => _run;
    private ReadOnlyReactiveProperty<bool> _run;

    public override IObservable<Vector2> Look => _look;

    public override IObservable<float> Scroll => throw new NotImplementedException();

    private IObservable<Vector2> _look;

    private InputActionMap _characterActionMap;
    private InputAction _characterMove;
    private InputAction _characterLook;
    private InputAction _characterJump;
    private InputAction _characterRun;

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
        if(inputActions is null)
        {
            Debug.LogError("FPSInputController requires inputActions to be assigned!");
            return;
        }

        _characterActionMap = inputActions.FindActionMap("Character", throwIfNotFound: true);
        _characterMove = _characterActionMap.FindAction("Move", throwIfNotFound: true);
        _characterLook = _characterActionMap.FindAction("Look", throwIfNotFound: true);
        _characterJump = _characterActionMap.FindAction("Jump", throwIfNotFound: true);
        _characterRun = _characterActionMap.FindAction("Run", throwIfNotFound: true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Move:
        _move = this.UpdateAsObservable()
            .Select(_ => _characterMove.ReadValue<Vector2>());

        // Jump:
        _jump = new Subject<Unit>().AddTo(this);
        _characterJump.performed += context => _jump.OnNext(Unit.Default);


        // Run:
        _run = this.UpdateAsObservable()
            .Select(_ => _characterRun.ReadValueAsObject() != null)
            .ToReadOnlyReactiveProperty();

        // Look:
        var smoothLookValue = new Vector2(0, 0);
        _look = this.UpdateAsObservable()
            .Select(_ =>
            {
                var rawLookValue = _characterLook.ReadValue<Vector2>();

                smoothLookValue = smoothLooking ? new Vector2(
                    Mathf.Lerp(smoothLookValue.x, rawLookValue.x, lookSmoothingFactor * Time.deltaTime),
                    Mathf.Lerp(smoothLookValue.y, rawLookValue.y, lookSmoothingFactor * Time.deltaTime)
                ) : rawLookValue;

                return smoothLookValue;
            });
    }
}
