using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents
{
    public abstract class GameEventListener<T> : MonoBehaviour
    {
        [SerializeField] private GameEventAsset<T> _gameEventAsset;
        [SerializeField] private bool _useFilter;
        [SerializeField] private T _filterValue;

        public UnityEvent<T> OnGameEventInvoked;

        private void OnEnable()
        {
            _gameEventAsset.OnInvoked.AddListener(GameEventInvoked);
        }

        private void OnDisable()
        {
            _gameEventAsset.OnInvoked.RemoveListener(GameEventInvoked);
        }

        private void GameEventInvoked(T param)
        {
            if (_useFilter && !param.Equals(_filterValue)) return;
            OnGameEventInvoked.Invoke(param);
        }
    }
}