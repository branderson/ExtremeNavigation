using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Movement options for each step of a Route
    /// </summary>
    public enum Move
    {
        Up,
        Down,
        Left,
        Right,
        Stay
    }

    /// <summary>
    /// Order of moves that the player takes through the city
    /// </summary>
    [Serializable]
    public class Route
    {
        [SerializeField] private List<Move> _moves;

        [NonSerialized] private int _movesIndex;

        public Route()
        {
            _moves = new List<Move>();
            _movesIndex = 0;
        }

        /// <summary>
        /// Add the given move to the end of the route
        /// </summary>
        /// <param name="move"></param>
        public void AddMove(Move move)
        {
            _moves.Add(move);
        }

        /// <summary>
        /// Advances to the next move in the Route and returns it
        /// </summary>
        /// <returns>
        /// The next move in the route
        /// </returns>
        public Move NextMove()
        {
            if (_movesIndex >= _moves.Count)
            {
                return Move.Stay;
            }
            Move next = _moves[_movesIndex];
            _movesIndex++;
            return next;
        }

        /// <summary>
        /// Remove all remaining moves from the Route, preserving moves already taken
        /// </summary>
        public void Reroute()
        {
            if (_movesIndex >= _moves.Count) return;
            _moves.RemoveRange(_movesIndex, _moves.Count - _movesIndex);
        }
    }
}
