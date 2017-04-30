using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.LevelElements
{
    public class RotateTowardsName : CustomMonoBehaviour
    {
        [SerializeField] private string _name;

        private Transform _target;

        private void Awake()
        {
            _target = GameObject.Find(_name).transform;
        }

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(_target.forward, _target.up);
        }
    }
}