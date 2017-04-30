using System.Collections.Generic;
using TiledLoader.Examples._3D.LevelElements;
using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine.SceneManagement;

namespace TiledLoader.Examples._3D.Managers
{
    public class DoorManager : Singleton<DoorManager>
    {
        private Dictionary<int, Door> _doors;

        protected DoorManager() { }

        private void Start()
        {
            SceneManager.sceneLoaded += OnLevelLoad;
        }

        private void OnLevelLoad(Scene scene, LoadSceneMode mode) 
        {
            LoadDoors();
        }

        private void LoadDoors()
        {
            _doors = new Dictionary<int, Door>();
            Door[] doors = FindObjectsOfType<Door>();

            foreach (Door door in doors)
            { 
                _doors[door.ID] = door;
            }
        }

        public Door GetDoor(int id)
        {
            if (_doors == null) LoadDoors();
            return id == 0 ? null : _doors[id];
        }
    }
}
