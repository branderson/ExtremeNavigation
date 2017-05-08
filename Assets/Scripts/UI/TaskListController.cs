using System.Collections.Generic;
using System.Linq;
using Controllers;
using Data;
using UI.Components;
using UnityEngine;

namespace UI
{
    public class TaskListController : MonoBehaviour
    {
        [SerializeField] private ScrollableSelectableList _availableList;
        [SerializeField] private ScrollableSelectableList _activeList;
        private List<Task> _availableTasks = new List<Task>();
        private List<Task> _activeTasks = new List<Task>();
        private Task _selected = null;

        private LevelController _levelController;

        private void Awake()
        {
            _levelController = FindObjectOfType<LevelController>();
        }

        public void ClearTasks()
        {
            _availableTasks.Clear();
            _activeTasks.Clear();
            RefreshAvailable();
            RefreshActive();
        }

        // TODO: Deselect on tab switch

        public void SelectTask(ISelectable taskButton)
        {
            // Get selected task
            string taskName = taskButton.GetText();
            Task task = _availableTasks.FirstOrDefault(item => item.Name == taskName) ?? _activeTasks.FirstOrDefault(item => item.Name == taskName);
            if (task == null) return;
            if (task == _selected)
            {
                // Deselect task
                taskButton.Deselect();
                _selected = null;

                // Disable minimap sprite
                task.DisableMinimapIfDisabled();
                task.DisableSpriteIfDisabled();
            }
            else
            {
                // Get current selection
                ISelectable current = _availableList.GetSelected().FirstOrDefault() ?? _activeList.GetSelected().FirstOrDefault();
                if (current != null)
                {
//                    string currentName = taskButton.GetText();
//                    Task currentTask = _availableTasks.FirstOrDefault(item => item.Name == currentName) ?? _activeTasks.FirstOrDefault(item => item.Name == currentName);
                    if (_selected != null) _selected.DisableMinimapIfDisabled();
                    if (_selected != null) _selected.DisableSpriteIfDisabled();
                    current.Deselect();
                }

                // Select task
                taskButton.Select();
                _selected = task;

                // Enable minimap sprite
                task.EnableSprite();
                task.EnableMinimap();
            }
        }

        public void DeselectTask()
        {
            // Get current selection
            ISelectable current = _availableList.GetSelected().FirstOrDefault() ?? _activeList.GetSelected().FirstOrDefault();
            if (current != null)
            {
                if (_selected != null) _selected.DisableMinimapIfDisabled();
                if (_selected != null) _selected.DisableSpriteIfDisabled();
                current.Deselect();
            }
            _selected = null;
        }

        public void ActivateTask()
        {
            if (_selected == null) return;
            // Activate selected task
            ActivateTask(_selected);
            // Needs to come after UI add in case task immediately completed
            _levelController.ActivateTask(_selected);
            _selected = null;
        }

        public void DeactivateTask()
        {
            if (_selected == null) return;
            _levelController.DeactivateTask(_selected);
            // Deactivate selected task
            DeactivateTask(_selected);
            _selected = null;
        }

        public void AddTasks(List<Task> tasks)
        {
            _availableTasks = new List<Task>(tasks);
            RefreshAvailable();
        }

        public void ActivateTask(Task task)
        {
            _availableTasks.Remove(task);
            _activeTasks.Add(task);
            RefreshAvailable();
            RefreshActive();
        }

        public void DeactivateTask(Task task)
        {
            _activeTasks.Remove(task);
            _availableTasks.Add(task);
            RefreshAvailable();
            RefreshActive();
        }

        public void CompleteTask(Task task)
        {
            _activeTasks.Remove(task);
            RefreshActive();
        }

        public void AddTask(Task task, bool active)
        {
            if (active)
            {
                _activeTasks.Add(task);
                RefreshActive();
            }
            else
            {
                _availableTasks.Add(task);
                RefreshAvailable();
            }
        }

        private void RefreshAvailable()
        {
            // Clear removed tasks
            _availableTasks.Remove(null);
            // Refresh UI
            _availableList.Populate(_availableTasks.Select(item => item.Name).ToList());
        }

        private void RefreshActive()
        {
            // Clear removed tasks
            _activeTasks.Remove(null);
            // Refresh UI
            _activeList.Populate(_activeTasks.Select(item => item.Name).ToList());
        }
    }
}