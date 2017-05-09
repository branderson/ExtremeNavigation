using System.Collections.Generic;
using System.Linq;
using Controllers;
using Data;
using TiledLoader;
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

        private TabbedPanelGroup _tabGroup;
        private LevelController _levelController;

        private void Awake()
        {
            _levelController = FindObjectOfType<LevelController>();
            _tabGroup = GetComponentInChildren<TabbedPanelGroup>();
        }

        private void Start()
        {
            RefreshAvailable();
            RefreshActive();
        }

        public void ClearTasks()
        {
            _availableTasks.Clear();
            _activeTasks.Clear();
            RefreshAvailable();
            RefreshActive();
        }

        public void SelectTask(Task task)
        {
            // Get current selection
            ISelectable current = _availableList.GetSelected().FirstOrDefault() ?? _activeList.GetSelected().FirstOrDefault();
            if (current != null)
            {
                if (_selected != null) _selected.Deselect();
                current.Deselect();
            }
            _selected = null;

            if (task == null) return;
            // Get selected task button. Search in available list
            ISelectable taskButton = _availableList.OptionObjects.FirstOrDefault(item => item.GetText().StartsWith(task.Name));
            if (taskButton == null)
            {
                // Search in active list
                taskButton = _activeList.OptionObjects.FirstOrDefault(item => item.GetText().StartsWith(task.Name));
                if (taskButton == null) return;
                // Make sure we're in the active list
                _tabGroup.OpenPanel(1);
            }
            else
            {
                // Make sure we're in the available list
                _tabGroup.OpenPanel(0);
            }
            if (task == _selected)
            {
                // Deselect task
                taskButton.Deselect();
                _selected = null;

                // Disable minimap sprite
                task.Deselect();
            }
            else
            {
                // Deselect current selection
                DeselectTask();

                // Select task
                taskButton.Select();
                _selected = task;

                // Enable minimap sprite
                task.Select();
            }
        }

        public void SelectTask(ISelectable taskButton)
        {
            // Get selected task
            string taskName = taskButton.GetText();
            Task task = _availableTasks.FirstOrDefault(item => taskName.StartsWith(item.Name)) ?? _activeTasks.FirstOrDefault(item => taskName.StartsWith(item.Name));
            if (task == null) return;
            if (task == _selected)
            {
                // Deselect task
                taskButton.Deselect();
                _selected = null;

                // Disable minimap sprite
                task.Deselect();
            }
            else
            {
                // Deselect current selection
                DeselectTask();

                // Select task
                taskButton.Select();
                _selected = task;

                // Enable minimap sprite
                task.Select();
            }
        }

        public void DeselectTask()
        {
            // Get current selection
            ISelectable current = _availableList.GetSelected().FirstOrDefault() ?? _activeList.GetSelected().FirstOrDefault();
            if (current != null)
            {
                if (_selected != null) _selected.Deselect();
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
            _availableList.Populate(_availableTasks.Select(item => TaskToString(item)).ToList());
        }

        private void RefreshActive()
        {
            // Clear removed tasks
            _activeTasks.Remove(null);
            // Refresh UI
            _activeList.Populate(_activeTasks.Select(item => TaskToString(item)).ToList());
        }

        private TiledLoaderPropertyData TaskToString(Task task)
        {
            // Add name and value
            TiledLoaderPropertyData properties = new TiledLoaderPropertyData();
            properties.SetProperty("Name", task.Name);
            properties.SetProperty("Value", task.Value);
            properties.SetProperty("Description", task.Description);
            properties.SetProperty("Count", task.Count);
            return properties;
        }
    }
}