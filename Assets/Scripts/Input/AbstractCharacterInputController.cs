using System;
using UniRx;
using UnityEngine;

public abstract class AbstractCharacterInputController
    : MonoBehaviour
{
    public abstract IObservable<Vector2> Move { get; }
    public abstract IObservable<Unit> Jump { get; }
    public abstract ReadOnlyReactiveProperty<bool> Run { get; }

    public abstract IObservable<Vector2> Look { get; }
    public abstract IObservable<float> Scroll { get; }
}