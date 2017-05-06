using UnityEngine.Events;

namespace Utility
{
    public class Events
    {
        [System.Serializable]
        public class StringEvent : UnityEvent<string> { }

        [System.Serializable]
        public class LongEvent : UnityEvent<long> { }

    }
}