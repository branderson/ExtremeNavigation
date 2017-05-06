using Data;
using UnityEngine;

namespace Controllers
{
    public class RoadTileController : MonoBehaviour
    {
        [SerializeField] private GameObject _availableSprite;
        [SerializeField] private GameObject _pathSprite;
        private RoadTile _roadTile;
        private LevelController _level;

        private bool _onPath = false;

        public int Time
        {
            get { return _roadTile.Time; }
        }

        private void Awake()
        {
            _roadTile = GetComponent<RoadTile>();
            _level = FindObjectOfType<LevelController>();
        }

        public bool GetConnected(RoadTileController roadTile)
        {
            return _roadTile.GetConnected(roadTile._roadTile);
        }

        public void MovedTo()
        {
            // TODO: Need to be able to undo, so markers need to be given back
            _onPath = true;
            SetPathSprite();
            // Trigger markers
            foreach (Marker marker in _roadTile.AdjacentMarkers)
            {
                if (marker == null) continue;
                marker.RouteEnter();
            }
            // Set available sprite for tiles we can move to
            foreach (RoadTile roadTile in _roadTile.GetEdges)
            {
                // TODO: Don't want to let us move backward
                if (roadTile == null) continue;
                roadTile.GetComponent<RoadTileController>().SetAvailableSprite();
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
            _pathSprite.SetActive(_onPath);
            _availableSprite.SetActive(false);
        }
    }
}