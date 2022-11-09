using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameEvents
{
    public abstract class GameEventCaller<T> : MonoBehaviour
    {
        [SerializeField] private GameEventRaiseFlag _raiseEvent = GameEventRaiseFlag.Start;
        [SerializeField] private T _value;
        [SerializeField] private GameEventAsset<T> _gameEventAsset;

        private void Awake()
        {
            if (HasEventFlag(GameEventRaiseFlag.Awake)) _gameEventAsset.Invoke(_value);
        }

        private void OnEnable()
        {
            if (HasEventFlag(GameEventRaiseFlag.OnEnable)) _gameEventAsset.Invoke(_value);
        }

        private void Start()
        {
            if (HasEventFlag(GameEventRaiseFlag.Start)) _gameEventAsset.Invoke(_value);
        }

        private void FixedUpdate()
        {
            if (HasEventFlag(GameEventRaiseFlag.FixedUpdate)) _gameEventAsset.Invoke(_value);
        }

        private void Update()
        {
            if (HasEventFlag(GameEventRaiseFlag.Update)) _gameEventAsset.Invoke(_value);
        }

        private void LateUpdate()
        {
            if (HasEventFlag(GameEventRaiseFlag.LateUpdate)) _gameEventAsset.Invoke(_value);
        }

        private void OnDisable()
        {
            if (HasEventFlag(GameEventRaiseFlag.OnDisable)) _gameEventAsset.Invoke(_value);
        }

        private void OnDestroy()
        {
            if (HasEventFlag(GameEventRaiseFlag.OnDestroy)) _gameEventAsset.Invoke(_value);
        }

        private bool HasEventFlag(GameEventRaiseFlag flag)
        {
            return (_raiseEvent & flag) == flag;
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
}