using TiledLoader.Examples._3D.LevelElements;
using TiledLoader.Examples._3D.Managers;
using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.Enemies
{
    public class EnemyAI : CustomMonoBehaviour, IAddressable
    {
        [SerializeField] private int _id;
        [SerializeField] private float _moveSpeed = 1.5f;        // The walking speed between waypoints
        public int Destination;

        private Waypoint _destination;            // Current waypoint

        bool _forward = true;            // direction
        double _turn = 6.0;                // Turn speed
//        float pause_time = 0;        // Pause at waypoint
        bool _pause = false;

        private CharacterController _character;
        private float _curTime;

        void Pause() { _pause = true; }

        void Unpause() { _pause = false; }

        void Reverse()
        {
            _forward = !_forward;
            ReachedWaypoint();
        }

        public void SetSpeed(long speed)
        {
            switch (speed)
            {
                case 1:
                    _moveSpeed = .5f;
                    break;
                case 2:
                    _moveSpeed = 1.5f;
                    break;
                case 3:
                    _moveSpeed = 3f;
                    break;
            }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private void Start()
        {
            _destination = WaypointManager.Instance.GetWaypoint(Destination);
            _character = GetComponent<CharacterController>();

            EventManager.Instance.StartListening("Pause", Pause);
            EventManager.Instance.StartListening("Unpause", Unpause);
            EventManager.Instance.StartListening("Reverse" + _id, Reverse);
            EventManager.Instance.StartListening("EnemySpeed" + _id, SetSpeed);
        }

        private void Update()
        {
            if (_pause) return;
            Patrol();
        }

        private void ReachedWaypoint()
        {
//            Debug.Log("Enemy " + _id + " reached waypoint " + _destination._id);
            if (_forward)
            {
                Destination = _destination.Next();
                if (Destination < 0)
                {
                    Destination *= -1;
                    _forward = false;
                }
            }
            else
            {
                Destination = _destination.Previous();
                if (Destination < 0)
                {
                    Destination *= -1;
                    _forward = true;
                }
            }
            _destination = WaypointManager.Instance.GetWaypoint(Destination);
        }

        public void WaypointTouched(Waypoint waypoint)
        {
            if (waypoint == _destination) ReachedWaypoint();
        }

        private void Patrol()
        {
            Vector3 target = _destination.transform.position;
      
            target.y = transform.position.y; // Keep waypoint at character's height
            Vector3 move_dir = target - transform.position;

            Quaternion rotation = Quaternion.LookRotation(target - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (float)_turn);
            _character.Move(move_dir.normalized * _moveSpeed * Time.deltaTime);
        }
    }
}
