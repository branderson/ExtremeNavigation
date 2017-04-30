using TiledLoader.Examples._3D.Player;
using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    public class Passcode : CustomMonoBehaviour
    {
        [SerializeField] private GameObject _mask;
        [SerializeField] private GameObject _passcodeText;

        public int ID;
        public string Code;

        private bool _activated = false;

        private void Start()
        {
            _mask.SetActive(true);
            _passcodeText.SetActive(false);
        }

        public void Interact()
        {
            if (!_activated)
            {
                _activated = true;
                _passcodeText.SetActive(true);
                _mask.SetActive(false);
                GameObject.Find("FlyingPlayer").GetComponent<FlyingPlayerController>().AddPasscode(ID, Code);
            }
        }
    }
}