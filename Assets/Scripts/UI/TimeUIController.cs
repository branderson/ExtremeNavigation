using Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimeUIController : MonoBehaviour
    {
        [SerializeField] private Text _timeText;

        public void SetTime(Timer time)
        {
            _timeText.text = "Time: " + time.Time.ToString();
        }
    }
}