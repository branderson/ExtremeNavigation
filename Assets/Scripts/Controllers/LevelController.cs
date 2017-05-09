using System.Collections.Generic;
using Data;
using Helpers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        private enum GameState
        {
            Routing,
            Running
        }

        [SerializeField] private Timer _timer;
        [SerializeField] private Rect _bounds;
        [SerializeField] private List<Task> _tasks;

        // Player state
        private GameState _state = GameState.Routing;
        private PlayerController _player;
        private RoadTileController _startingPosition;
        private LinkedList<RoadTileController> _path;
        // TODO: Might not need this route
//        private Route _route;
        private int _money = 0;

        private Camera _gameCamera;
//        private RoadTileController[] _roadTiles;
        private TaskListController _taskList;
        private TimeUIController _timeUI;
        private MoneyUIController _moneyUI;

        /// <summary>
        /// Bounds of tilemap in world coordinates
        /// </summary>
        [SerializeField]
        public Rect Bounds
        {
            get { return _bounds; }
            set
            {
                _bounds = new Rect(value);
            }
        }

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
            _startingPosition = _player.CurrentPosition;
//            _route = new Route();
            _gameCamera = FindObjectOfType<GameCameraController>().GetComponent<Camera>();
            _timeUI = FindObjectOfType<TimeUIController>();
            _moneyUI = FindObjectOfType<MoneyUIController>();

            // Move camera
//            _gameCamera.transform.position = _player.transform.position;
        }

        private void Start()
        {
            _path = new LinkedList<RoadTileController>();
            _path.AddFirst(_startingPosition);

            _taskList = FindObjectOfType<TaskListController>();
            _taskList.AddTasks(_tasks);

            // Initialize UI
            _timeUI.SetTime(_timer);
            _moneyUI.SetMoney(_money);

            // Initialize starting position
            _startingPosition.MovedTo(Move.Stay, _timer.Time);
        }

        private void Update()
        {
            switch (_state)
            {
                case GameState.Routing:
                    // Allow path drawing while left mouse button down
                    if (Input.GetMouseButton(0))
                    {
                        DrawPath();
                    }

                    // Allow undo
                    if (Input.GetMouseButtonDown(1))
                    {
                        PopPath();
                    }
                    break;
                case GameState.Running:
                    break;
            }
        }

        private void DrawPath()
        {
            // Find RoadTile under mouse
            RoadTileController roadTile = null;
            Vector3 mousePosition = Input.mousePosition;
            // Check for all colliding objects
            Collider2D[] cols = Physics2D.OverlapPointAll(_gameCamera.ScreenToWorldPoint(mousePosition));
            foreach (Collider2D col in cols)
            {
                // Check each colliding object to find RoadTile
                roadTile = col.GetComponent<RoadTileController>();
            }

            // Attempt to move to tile
            if (roadTile == null) return;

            // Check if we have enough time
            if (_timer.Time < roadTile.Time) return;

            RoadTileController current = _path.Last.Value;
            // Make sure not moving to previous position
            if (_path.Last.Previous != null && roadTile == _path.Last.Previous.Value) return;
            // Check if we can move here
            if (!current.GetConnected(roadTile)) return;

            // Get move direction
            Move move = current.GetMove(roadTile);
            if (move == Move.Stay) return;

            // Cost time
            _timer.SubtractTime(roadTile.Time);
            _timeUI.SetTime(_timer);

            // Update drawing UI
            current.UnsetSurroundingAvailable();
            roadTile.MovedTo(move, _timer.Time);

            // Add to path
            _path.AddLast(roadTile);

            // TODO: Remove route stuff?
            // Add to route
//            _route.AddMove(move);

            if (_timer.Time <= 0) TimeElapsed();
        }

        private void PopPath()
        {
            // Make sure we're not at the start
            if (_path.Count == 1) return;

            // Reset money for recalcuation
            _money = 0;

            _path.Last.Value.Pop();
            _path.RemoveLast();

            // Get direction between last two
            Move move;
            RoadTileController current = _path.Last.Value;
            LinkedListNode<RoadTileController> prev = _path.Last.Previous;
            if (prev == null) move = Move.Stay;
            else move = prev.Value.GetMove(current);

            // Update UI
            RecalcuateTime();
            _moneyUI.SetMoney(_money);

            // Set new head as path head
            _path.Last.Value.SetHead(move, _timer.Time);
            EventManager.Instance.TriggerEvent("PopTile");
        }

        public void RerunPath()
        {
            // Iterate through path
            for (LinkedListNode<RoadTileController> node = _path.First; node != null; node = node.Next)
            {
                RoadTileController tile = node.Value;
                tile.TriggerMarkers();
            }
        }

        public void RunPath()
        {
            _state = GameState.Running;
            _player.Run();
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void RecalcuateTime()
        {
            _timer.ResetTime();
            // Iterate through path
            for (LinkedListNode<RoadTileController> node = _path.First; node != null; node = node.Next)
            {
                RoadTileController tile = node.Value;
                // Make sure it's not the first tile
                if (tile != _startingPosition)
                {
                    // Subtract time for each tile
                    _timer.SubtractTime(tile.Time);
                }
            }
            _timeUI.SetTime(_timer);
        }

        private void TimeElapsed()
        {
            Debug.Log("Time Elapsed");
        }

        /// <summary>
        /// Initialize the game timer to the given value
        /// </summary>
        /// <param name="time">
        /// Time to allot to level
        /// </param>
        public void InitializeTimer(int time)
        {
            _timer = new Timer(time);
        }

        /// <summary>
        /// Register the given task with the level
        /// </summary>
        /// <param name="task">
        /// Task to register
        /// </param>
        public void AddTask(Task task)
        {
            if (_tasks == null) _tasks = new List<Task>();
            _tasks.Add(task);
            task.LevelController = this;
        }

        public void ActivateTask(Task task)
        {
            if (task == null) return;
            task.Enable();
        }

        public void DeactivateTask(Task task)
        {
            if (task == null) return;
            task.Disable();
        }

        /// <summary>
        /// Complete the given task and earn its reward
        /// </summary>
        /// <param name="task">
        /// Task to complete
        /// </param>
        public void CompleteTask(Task task)
        {
            // Get money
            _money += task.Value;
            _moneyUI.SetMoney(_money);

            // Disable task
            _taskList.CompleteTask(task);
        }

        public void UncompleteTask(Task task, bool active)
        {
            // Remove money
//            _money -= task.Value;
            _moneyUI.SetMoney(_money);

            // Move back to list
            _taskList.AddTask(task, active);
        }
    }
}