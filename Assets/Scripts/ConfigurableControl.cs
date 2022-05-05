using System;
using UniRx;
using UnityEngine;

public abstract class ConfigurableControl
    : MonoBehaviour
{
    public abstract IObservable<Unit> ControlRequested { get; }
}
