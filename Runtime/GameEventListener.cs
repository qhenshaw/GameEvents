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
        [SerializeField] private T[] _filterValues;

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
            if (_useFilter && !TestFilter(param)) return;
            OnGameEventInvoked.Invoke(param);
        }

        private bool TestFilter(T param)
        {
            foreach (T filterValue in _filterValues)
            {
                if(param.Equals(filterValue)) return true;
            }

            return false;
        }
    }
}