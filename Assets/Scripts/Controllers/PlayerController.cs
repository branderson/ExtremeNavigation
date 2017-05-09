using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = .5f; // Tiles moved per second
        public RoadTileController CurrentPosition = null;

        private LinkedList<RoadTileController> _path = null;
        private LevelController _levelController;
        private RoadTileController _next = null;
        private bool _running = false;
        private float _rate = 0;
        private float _elapsed = 0;

        private void Awake()
        {
            // TODO: Do this on load
            // Check for all colliding objects
            Collider2D[] cols = Physics2D.OverlapPointAll(transform.position);
            foreach (Collider2D col in cols)
            {
                // Check each colliding object to find layout element
                RoadTileController roadTile = col.GetComponent<RoadTileController>();
                if (roadTile != null)
                {
                    CurrentPosition = roadTile;
                    _next = roadTile;
                }
            }

            _levelController = FindObjectOfType<LevelController>();
        }

        private void Update()
        {
            if (!_running) return;
            _elapsed += Time.deltaTime;

            // Move along path
            float z = transform.position.z;
            transform.position = Vector3.Lerp(CurrentPosition.transform.position, _next.transform.position, (_speed * _rate) * _elapsed);
            transform.position = new Vector3(transform.position.x, transform.position.y, z);

            // Start next movement
            if (_elapsed > 1 / (_rate * _speed))
            {
                CurrentPosition = _next;

                // Reset timer
                _elapsed = 0;

                AdvancePosition();

                // Check if path complete
                if (_path.Count == 0)
                {
                    // End level
                    _running = false;
                    _levelController.EndLevel();
                }
            }
        }

        public void Run(LinkedList<RoadTileController> path)
        {
            _running = true;
            _path = new LinkedList<RoadTileController>(path);
            // Pop the first value of the path and initialize next
            _path.RemoveFirst();
            AdvancePosition();
        }

        private void AdvancePosition()
        {
            if (_path.First == null) return;
            // Move a position
            _next = _path.First.Value;
            Move move = CurrentPosition.GetMove(_next);
            _path.RemoveFirst();
            // Get rate through position
            _rate = 1f / _next.Time;
            MoveRotate(move);
        }

        public void MoveTo(RoadTileController tile, Move move)
        {
            transform.position = tile.transform.position;
            MoveRotate(move);
        }

        private void MoveRotate(Move move)
        {
            switch (move)
            {
                case Move.Up:
                    transform.rotation = Quaternion.identity;
                    break;
                case Move.Down:
                    transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                    break;
                case Move.Left:
                    transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                    break;
                case Move.Right:
                    transform.rotation = Quaternion.AngleAxis(270, Vector3.forward);
                    break;
            }
        }

//        public Move MoveTo(RoadTileController tile)
//        {
//            if (!CanMoveTo(tile)) return Move.Stay;
//            Move move = CurrentPosition.GetMove(tile);
//            if (move == Move.Stay) return Move.Stay;
//            CurrentPosition.UnsetSurroundingAvailable();
//            CurrentPosition = tile;
//            CurrentPosition.MovedTo();
//            transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, transform.position.z);
//            return move;
//        }

//        public bool CanMoveTo(RoadTileController tile)
//        {
//            return CurrentPosition.GetConnected(tile);
//        }
    }
}