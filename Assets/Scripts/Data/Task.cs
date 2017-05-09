using Controllers;
using UnityEngine;
using Utility;

namespace Data
{
    public class Task : MonoBehaviour
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [SerializeField] public int Value;
        [SerializeField] public int Count;
        [SerializeField] private Marker _head;
        [SerializeField] private LevelController _levelController;

        private Marker _start;
        private bool _enabled = false;
        private bool _complete = false;

        public LevelController LevelController
        {
            set { _levelController = value; }
        }

        /// <summary>
        /// Current marker in task
        /// </summary>
        public Marker Head
        {
            set { _head = value; }
        }

        private void Awake()
        {
            EventManager.Instance.StartListening("PopTile", Recalculate);
            _start = _head;
            Marker node = _start;
            while (node != null)
            {
                Count++;
                node = node.Next;
            }
        }

        private void Start()
        {
            Disable();
        }


        /// <summary>
        /// Enable the markers for this task
        /// </summary>
        public void Enable()
        {
            _head.Activate();
            _enabled = true;
            _levelController.RerunPath();
        }

        /// <summary>
        /// Disable the markers for this task
        /// </summary>
        public void Disable()
        {
            _start.Deactivate();
            _head = _start;
            _enabled = false;
            _complete = false;
        }

        /// <summary>
        /// Select the markers for this task
        /// </summary>
        public void Select()
        {
            _start.Select();
        }

        /// <summary>
        /// Deselect the markers for this task
        /// </summary>
        public void Deselect()
        {
            _start.Deselect();
        }

//        public void EnableSprite()
//        {
//        }

//        public void DisableSpriteIfDisabled()
//        {
//            if (!_enabled) _start.DisableSpriteRecursive();
//        }

//        public void EnableMinimap()
//        {
//            _start.EnableMinimapRecursive();
//        }

//        public void DisableMinimapIfDisabled()
//        {
//            if (!_enabled) _start.DisableMinimapRecursive();
//        }

        public void Recalculate()
        {
            // Revert to starting state, maintaining enabled state
            _start.Deactivate();
            TaskUncomplete();
            _head = _start;
            // Recalculate completion if enabled
            if (_enabled) Enable();
        }

        /// <summary>
        /// Check if the given marker can be triggered
        /// </summary>
        /// <returns>
        /// Whether marker trigger is valid
        /// </returns>
        public bool TriggerMarker(Marker marker)
        {
            // Make sure marker is next in line
            return marker == _head;
        }

        /// <summary>
        /// Complete the task
        /// </summary>
        public void TaskComplete()
        {
            if (_complete) return;
            _complete = true;
            _levelController.CompleteTask(this);
        }

        public void TaskUncomplete()
        {
            if (_complete)
            {
                _levelController.UncompleteTask(this, _enabled);
            }
            _complete = false;
        }
    }
}