using Controllers;
using Data;
using TiledLoader;
using UnityEngine;

namespace ImportControllers
{
    [ExecuteInEditMode]
    public class TaskLayerImporter : MonoBehaviour
    {
        private void HandleLayerProperties()
        {
            ImportTask();
            ImportMarkers();
            DestroyImmediate(GetComponent<TiledLoaderProperties>());
            DestroyImmediate(this, true);
        }

        private void ImportTask()
        {
            // Get reference to TiledLoaderProperties
            TiledLoaderProperties properties = GetComponent<TiledLoaderProperties>();

            // Import task data
            Task task = GetComponent<Task>();
            task.Name = name;
            properties.TryGetString("Description", out task.Description);
            properties.TryGetInt("Value", out task.Value);
        }

        private void ImportMarkers()
        {
            Task task = GetComponent<Task>();
            Marker start = null;
            Marker mid = null;
            Marker end = null;

            // Iterate through children collecting markers
            foreach (Marker marker in transform.GetComponentsInChildren<Marker>())
            {
                marker.Task = task;
                if (marker.First)
                {
                    start = marker;
                }
                else if (marker.Middle)
                {
                    mid = marker;
                }
                else if (marker.End)
                {
                    end = marker;
                }
            }

            // All tasks must have an end marker
            if (end == null) return;
            // Make end the first marker
            task.Head = end;
            if (mid != null)
            {
                mid.Next = end;
                // Replace end with mid as first marker
                task.Head = mid;
            }
            if (start != null)
            {
                // Tasks can either have a start, mid, and end...
                if (mid != null) start.Next = mid;
                // Or a start and end
                else start.Next = end;
                // Or just an end
                // Replace mid/end with start as first marker
                task.Head = start;
            }

            // Add task to level
            LevelController level = FindObjectOfType<LevelController>();
            level.AddTask(task);
        }
    }
}