using System.Collections.Generic;
using TiledLoader.Examples._3D.Managers;
using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    public class Switch : CustomMonoBehaviour, IAddressable
    {
        [SerializeField] private int _id;
        public List<int> DoorIDs = new List<int>();

        private AudioSource _source;
        private MeshRenderer _renderer;
        private bool _on = false;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private void Awake()
        {
            _renderer = GetComponentInChildren<MeshRenderer>();
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            foreach (int id in DoorIDs)
            {
                DoorManager.Instance.GetDoor(id).SwitchControlled = true;
            }
        }

        public void Toggle()
        {
            if (_on) return;
            _source.Play();
            foreach (int id in DoorIDs)
            {
                DoorManager.Instance.GetDoor(id).Toggle();
            }
            // Rotate to flip switch position
            _renderer.transform.Rotate(new Vector3(0, 0, 180));
            _on = !_on;
        }

        public void Interact()
        {
            Toggle();
        }
    }
}