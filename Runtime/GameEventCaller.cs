using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventCaller<T> : MonoBehaviour
{
    [SerializeField] private GameEventRaiseFlag _raiseEvent = GameEventRaiseFlag.Start;
    [SerializeField] private T _value;
    [SerializeField] private GameEvent<T> _event;

    private void Awake()
    {
        if (HasEventFlag(GameEventRaiseFlag.Awake)) _event.Invoke(_value);
    }

    private void OnEnable()
    {
        if (HasEventFlag(GameEventRaiseFlag.OnEnable)) _event.Invoke(_value);
    }

    private void Start()
    {
        if(HasEventFlag(GameEventRaiseFlag.Start)) _event.Invoke(_value);
    }

    private void FixedUpdate()
    {
        if (HasEventFlag(GameEventRaiseFlag.FixedUpdate)) _event.Invoke(_value);
    }

    private void Update()
    {
        if (HasEventFlag(GameEventRaiseFlag.Update)) _event.Invoke(_value);
    }

    private void LateUpdate()
    {
        if (HasEventFlag(GameEventRaiseFlag.LateUpdate)) _event.Invoke(_value);
    }

    private void OnDisable()
    {
        if (HasEventFlag(GameEventRaiseFlag.OnDisable)) _event.Invoke(_value);
    }

    private void OnDestroy()
    {
        if (HasEventFlag(GameEventRaiseFlag.OnDestroy)) _event.Invoke(_value);
    }

    private bool HasEventFlag(GameEventRaiseFlag flag)
    {
        return ((_raiseEvent & flag) == flag);
    }

    [System.Flags]
    public enum GameEventRaiseFlag
    {
        Awake = 1 << 1,
        OnEnable = 1 << 2,
        Start = 1 << 3,
        FixedUpdate = 1 << 4,
        Update = 1 << 5,
        LateUpdate = 1 << 6,
        OnDisable = 1 << 7,
        OnDestroy = 1 << 8
    }
}
