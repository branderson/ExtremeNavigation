using System;
using UnityEngine;

namespace Helpers
{
    [Serializable]
    public class Timer
    {
        [SerializeField] private int _timeRemaining = 0;
        [SerializeField] private int _startingTime = 0;

        public Timer(int time)
        {
            _timeRemaining = time;
            _startingTime = time;
        }

        public int Time
        {
            get { return _timeRemaining; }
        }

        /// <summary>
        /// Subtract the given amount of time from the timer's remaining time
        /// </summary>
        /// <param name="elapsed">
        /// Amount of time units to subtract
        /// </param>
        public void SubtractTime(int elapsed)
        {
            _timeRemaining -= elapsed;
            if (_timeRemaining <= 0)
            {
                _timeRemaining = 0;
                TimeElapsed();
            }
        }

        /// <summary>
        /// Reset remaining time to initial value
        /// </summary>
        public void ResetTime()
        {
            _timeRemaining = _startingTime;
        }

        private void TimeElapsed()
        {
            
        }
    }
}