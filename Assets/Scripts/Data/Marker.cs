using UnityEngine;

namespace Data
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] public bool Active;
        [SerializeField] public bool Start;
        [SerializeField] public bool Middle;
        [SerializeField] public bool End;
        [SerializeField] public Marker Next = null;
        [SerializeField] public Task Task = null;

        /// <summary>
        /// Enable the marker list from here on
        /// </summary>
        public void Enable()
        {
            Active = true;
            gameObject.SetActive(true);
            if (Next != null)
            {
                // Enable list
                Next.Enable();
            }
        }

        /// <summary>
        /// Disable the marker list from here on
        /// </summary>
        public void Disable()
        {
            Active = false;
            gameObject.SetActive(false);
            if (Next != null)
            {
                // Disable list
                Next.Disable();
            }
        }

        /// <summary>
        /// Attempt to complete marker
        /// </summary>
        public void RouteEnter()
        {
            Debug.Log("RouteEnter " + Task.Name);
            // Make sure this marker is active
            if (!Active) return;
            Debug.Log("Active " + Task.Name);
            // Make sure this marker is next
            if (!Task.TriggerMarker(this)) return;
            Debug.Log("Triggered " + Task.Name);
            // If this is the end, we've completed the task
            if (Next == null) Task.TaskComplete();
            // Otherwise, move down the chain
            Task.Head = Next;
            // Deactivate this marker
            Active = false;
            gameObject.SetActive(false);
        }
    }
}