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
            Disable();
        }


        /// <summary>
        /// Enable the markers for this task
        /// </summary>
        public void Enable()
        {
            _head.Enable();
            _enabled = true;
            _levelController.RerunPath();
        }

        /// <summary>
        /// Disable the markers for this task
        /// </summary>
        public void Disable()
        {
            _start.Disable();
            _head = _start;
            _enabled = false;
            _complete = false;
        }

        public void Recalculate()
        {
            // Revert to starting state, maintaining enabled state
            _start.Disable();
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