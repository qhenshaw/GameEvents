using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEvent<T> : ScriptableObject
{
    [SerializeField] private bool _log = false;
    [SerializeField] private T _currentValue;

    public T CurrentValue => _currentValue;

    public UnityEvent<T> OnInvoked;

    public void Invoke(T param)
    {
        if(_log) Debug.Log($"{name} event invoked: {param}", this);
        _currentValue = param;
        OnInvoked.Invoke(param);
    }
}