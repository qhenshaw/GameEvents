using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents
{
    public abstract class GameEventAsset<T> : ScriptableObject
    {
        [SerializeField] private bool _log = false;
        [SerializeField] private T _currentValue;
        [SerializeField] private T _testValue;
        [SerializeField] private bool _autoCleanupListeners = true;

        public T CurrentValue => _currentValue;
        private List<UnityAction<T>> _listeners = new List<UnityAction<T>>();

        public UnityEvent<T> OnInvoked;

        public void Invoke(T param)
        {
            if (_log) Debug.Log($"{name} event invoked: {param}", this);
            _currentValue = param;
            OnInvoked.Invoke(param);
        }

        public void AddListener(UnityAction<T> listener)
        {
            object target = listener.Target;
            if (_log) Debug.Log($"Event [{name}] added listener [{target}.{listener.Method.Name}]", this);
            OnInvoked.AddListener(listener);
            _listeners.Add(listener);

            if(_autoCleanupListeners) CleanupListeners();
        }

        public void RemoveListener(UnityAction<T> listener)
        {
            object target = listener.Target;
            if (_log) Debug.Log($"Event [{name}] removed listener [{target}.{listener.Method.Name}]", this);
            OnInvoked.RemoveListener(listener);
            if(_listeners.Contains(listener)) _listeners.Remove(listener);
        }

        private void CleanupListeners()
        {
            List<UnityAction<T>> toRemove = new List<UnityAction<T>>();
            foreach (UnityAction<T> listener in _listeners)
            {
                if(listener == null || listener.Target.Equals(null)) toRemove.Add(listener);
            }

            foreach (UnityAction<T> listener in toRemove)
            {
                _listeners.Remove(listener);
            }
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        private void Test()
        {
            Invoke(_testValue);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        private void LogListeners()
        {
            foreach (UnityAction<T> listener in _listeners)
            {
                object target = listener.Target;
                if (target is Object)
                {
                    Debug.Log($"{target}.{listener.Method.Name}", target as Object);
                }
                else
                {
                    Debug.Log($"{target}.{listener.Method.Name}");
                }
            }
        }
    }
}