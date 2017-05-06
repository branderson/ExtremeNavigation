using System.Collections.Generic;
using Data;
using Helpers;
using UI;
using UnityEngine;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private Timer _timer;
        [SerializeField] private Rect _bounds;
        [SerializeField] private List<Task> _tasks;

        // Player state
        private PlayerController _player;
        private List<RoadTileController> _path;
        // TODO: Might not need this route
        private Route _route;
        private int _money = 0;

        private Camera _gameCamera;
        private RoadTileController[] _roadTiles;
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
            _route = new Route();
            _gameCamera = FindObjectOfType<GameCameraController>().GetComponent<Camera>();
            _roadTiles = FindObjectsOfType<RoadTileController>();
            _timeUI = FindObjectOfType<TimeUIController>();
            _moneyUI = FindObjectOfType<MoneyUIController>();
        }

        private void Start()
        {
            _path = new List<RoadTileController> {_player.CurrentPosition };

            _taskList = FindObjectOfType<TaskListController>();
            _taskList.AddTasks(_tasks);

            // Initialize starting position
            _player.CurrentPosition.MovedTo();

            // Initialize UI
            _timeUI.SetTime(_timer);
            _moneyUI.SetMoney(_money);
        }

        private void Update()
        {
            // Allow path drawing while left mouse button down
            if (Input.GetMouseButton(0))
            {
                // Find RoadTile under mouse
                Vector3 mousePosition = Input.mousePosition;
                // Check for all colliding objects
                Collider2D[] cols = Physics2D.OverlapPointAll(_gameCamera.ScreenToWorldPoint(mousePosition));
                foreach (Collider2D col in cols)
                {
                    // Check each colliding object to find RoadTile
                    RoadTileController roadTile = col.GetComponent<RoadTileController>();
                    if (roadTile == null) continue;

                    // Check if we can move here and move if possible
                    if (_timer.Time >= roadTile.Time)
                    {
                        Move move = _player.MoveTo(roadTile);
                        if (move != Move.Stay)
                        {
                            // Cost time
                            _timer.SubtractTime(roadTile.Time);
                            _timeUI.SetTime(_timer);

                            _route.AddMove(move);

                            if (_timer.Time <= 0) TimeElapsed();
                        }
                    }
                }
            }
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
            // TODO: Need to recalculate completion on task activation/deactivation
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
            task.Disable();
            _taskList.CompleteTask(task);
        }
    }
}