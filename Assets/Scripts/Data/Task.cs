using Controllers;
using UnityEngine;

namespace Data
{
    public class Task : MonoBehaviour
    {
        [SerializeField] public string Name;
        [SerializeField] public string Description;
        [SerializeField] public int Value;
        [SerializeField] private Marker _head;
        [SerializeField] private LevelController _levelController;

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
            Disable();
        }


        /// <summary>
        /// Enable the markers for this task
        /// </summary>
        public void Enable()
        {
            _head.Enable();
        }

        /// <summary>
        /// Disable the markers for this task
        /// </summary>
        public void Disable()
        {
            _head.Disable();
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
            _levelController.CompleteTask(this);
        }
    }
}