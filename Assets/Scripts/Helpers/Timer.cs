namespace Helpers
{
    public class Timer
    {
        private int _timeRemaining = 0;

        public Timer(int time)
        {
            _timeRemaining = time;
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

        private void TimeElapsed()
        {
            
        }
    }
}