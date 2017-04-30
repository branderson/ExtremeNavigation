using TiledLoader.Examples._3D.Utility.BaseClasses;

namespace TiledLoader.Examples._3D.LevelElements
{
    public class Endpoint : CustomMonoBehaviour
    {
        private bool _activated = false;

        private FloatingID _floatingID;

        private void Awake()
        {
            _floatingID = GetComponentInChildren<FloatingID>();
        }

        private void Start()
        {
            _floatingID.SetText("Complete Mission");
        }

        public void Interact()
        {
            if (!_activated)
            {
                _activated = true;
                _floatingID.SetText("Congratulations, You Won!");
            }
        }
    }
}