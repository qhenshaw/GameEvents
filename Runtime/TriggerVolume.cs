using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GameEvents
{
    [RequireComponent(typeof(BoxCollider))]
    public class TriggerVolume : MonoBehaviour
    {
        [SerializeField] private string _tagFilter = "Player";
        [SerializeField] private bool _doOnce = true;
        [field: SerializeField] public bool Done { get; set; }

        public UnityEvent<GameObject> OnEnter;
        public UnityEvent<GameObject> OnExit;


        // ensure box collider is added, and is a trigger
        private void OnValidate()
        {
            if (TryGetComponent(out BoxCollider collider))
            {
                collider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (CheckForTagMatch(other.gameObject) && (!_doOnce || !Done))
            {
                OnEnter.Invoke(other.gameObject);
                Done = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (CheckForTagMatch(other.gameObject) && (!_doOnce || !Done))
            {
                OnExit.Invoke(other.gameObject);
                Done = true;
            }
        }

        private bool CheckForTagMatch(GameObject other)
        {
            // ignore tag matching if tag is empty
            if (string.IsNullOrEmpty(_tagFilter)) return true;
            // compare tags
            return other.CompareTag(_tagFilter);
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Volume/Trigger Volume", false, 0)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            GameObject go = new GameObject("Trigger Volume");
            go.AddComponent<TriggerVolume>();
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }
#endif
    }
}