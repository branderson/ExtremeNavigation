using Data;
using UnityEngine;
using Utility;

namespace Controllers
{
    public class RoadTileController : MonoBehaviour
    {
        [SerializeField] private GameObject _availableSprite;
        [SerializeField] private GameObject _pathSprite;
        private RoadTile _roadTile;
//        private LevelController _level;

        private int _onPath = 0;

        public int Time
        {
            get { return _roadTile.Time; }
        }

        private void Awake()
        {
            _roadTile = GetComponent<RoadTile>();
//            _level = FindObjectOfType<LevelController>();
        }

        public bool GetConnected(RoadTileController roadTile)
        {
            return _roadTile.GetConnected(roadTile._roadTile);
        }

        public void MovedTo(Move move, int timeRemaining)
        {
            _onPath += 1;
            SetHead(move, timeRemaining);
            TriggerMarkers();
        }

        public void SetHead(Move move, int timeRemaining)
        {
            SetPathSprite();
            // Set available sprite for tiles we can move to
            // TODO: This needs to be changed
            RoadTile[] edges = (RoadTile[])_roadTile.GetEdges.Clone();
            // TODO: Use directions instead to clean this up
            if (move == Move.Up) edges[(int) RoadTile.Direction.South] = null;
            if (move == Move.Down) edges[(int) RoadTile.Direction.North] = null;
            if (move == Move.Right) edges[(int) RoadTile.Direction.West] = null;
            if (move == Move.Left) edges[(int) RoadTile.Direction.East] = null;
            foreach (RoadTile roadTile in edges)
            {
                if (roadTile == null || roadTile.Time > timeRemaining) continue;
                roadTile.GetComponent<RoadTileController>().SetAvailableSprite();
            }
        }

        public void Pop()
        {
            _onPath -= 1;
            SetNormalSprite();
            UnsetSurroundingAvailable();
        }

        public void TriggerMarkers()
        {
            if (_onPath == 0) return;
            // Trigger markers
            foreach (Marker marker in _roadTile.AdjacentMarkers)
            {
                if (marker == null) continue;
                marker.RouteEnter();
            }
        }

        public void UnsetSurroundingAvailable()
        {
            foreach (RoadTile roadTile in _roadTile.GetEdges)
            {
                if (roadTile == null) continue;
                roadTile.GetComponent<RoadTileController>().UnsetAvailableSprite();
            }
        }

        /// <summary>
        /// Get the direction that we move to get from here to the given tile
        /// </summary>
        /// <param name="tile">
        /// Tile to get move direction to
        /// </param>
        /// <returns>
        /// Direction we move to get to tile from here
        /// </returns>
        public Move GetMove(RoadTileController tile)
        {
            return _roadTile.GetMove(tile._roadTile);
        }

        private void SetNormalSprite()
        {
            _pathSprite.SetActive(false);
            _availableSprite.SetActive(false);
        }

        private void SetPathSprite()
        {
            _pathSprite.SetActive(true);
            _availableSprite.SetActive(false);

            // Set connected sprites to available
        }

        private void SetAvailableSprite()
        {
            _availableSprite.SetActive(true);
            _pathSprite.SetActive(false);
        }

        private void UnsetAvailableSprite()
        {
            _pathSprite.SetActive(_onPath != 0);
            _availableSprite.SetActive(false);
        }
    }
}