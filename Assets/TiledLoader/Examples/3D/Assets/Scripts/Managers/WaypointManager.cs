using System.Collections.Generic;
using TiledLoader.Examples._3D.LevelElements;
using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine.SceneManagement;

namespace TiledLoader.Examples._3D.Managers
{
    public class WaypointManager : Singleton<WaypointManager>
    {
        private Dictionary<int, Waypoint> _waypoints;

        protected WaypointManager() { }

        private void Start()
        {
            SceneManager.sceneLoaded += OnLevelLoad;
        }

        private void OnLevelLoad(Scene scene, LoadSceneMode mode) 
        {
            LoadWaypoints();
        }

        private void LoadWaypoints()
        {
            _waypoints = new Dictionary<int, Waypoint>();
            Waypoint[] wps = FindObjectsOfType<Waypoint>();

            foreach (Waypoint wp in wps)
            { 
                _waypoints[wp.ID] = wp;
            }
        }

        public Waypoint GetWaypoint(int id)
        {
            if (_waypoints == null) LoadWaypoints();
            return id == 0 ? null : _waypoints[id];
        }
    }
}
