using UnityEngine;

namespace Data
{
    public class RoadTile : MonoBehaviour
    {
        /// <summary>
        /// Directions that roads can connect along
        /// </summary>
        public enum Direction
        {
            North = 0,
            South = 1,
            East = 2,
            West = 3
        }

        [SerializeField] private bool[] _directions = {false, false, false, false};
        [SerializeField] private RoadTile[] _edges = {null, null, null, null};
        [SerializeField] private int _time = 1;

        /// <summary>
        /// Add the given amount of time to the time required to traverse this tile
        /// </summary>
        /// <param name="time">
        /// Time to add to tile's time cost
        /// </param>
        public void AddTime(int time)
        {
            _time += time;
        }

        /// <summary>
        /// Set whether the road can connect along the given direction
        /// </summary>
        /// <param name="direction">
        /// Direction to set availability of
        /// </param>
        /// <param name="available">
        /// Whether the road can connect along this direction
        /// </param>
        public void SetDirection(Direction direction, bool available)
        {
            _directions[(int) direction] = available;
        }

        /// <summary>
        /// Get whether the road can connect along the given direction
        /// </summary>
        /// <param name="direction">
        /// Direction to check availability of
        /// </param>
        /// <returns>
        /// Whether the road can connect along this direction
        /// </returns>
        public bool GetDirection(Direction direction)
        {
            return _directions[(int) direction];
        }

        /// <summary>
        /// Connect the given tile to this tile along the given direction
        /// </summary>
        /// <param name="direction">
        /// Direction from this tile to connection
        /// </param>
        /// <param name="connection">
        /// Tile to connect to this tile
        /// </param>
        public void ConnectEdge(Direction direction, RoadTile connection)
        {
            _edges[(int) direction] = connection;
        }
    }
}