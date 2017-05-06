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
            }
            else
            {
                // Get current selection
                ISelectable current = _availableList.GetSelected().FirstOrDefault() ?? _activeList.GetSelected().FirstOrDefault();
                if (current != null)
                {
                    current.Deselect();
                }

                // Select task
                taskButton.Select();
                _selected = task;
            }
        }

        public void ActivateTask()
        {
            if (_selected == null) return;
            _levelController.ActivateTask(_selected);
            // Activate selected task
            ActivateTask(_selected);
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