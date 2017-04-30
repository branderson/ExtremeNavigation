using TiledLoader.Examples._3D.Player;
using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TiledLoader.Examples._3D.Managers
{
    /// <summary>
    /// Manages display and input configuration for development
    /// </summary>
    public class DisplayManager : Singleton<DisplayManager>
    {
        [SerializeField] private Camera _walkingCamera;
        [SerializeField] private Camera _flyingCamera;
        [SerializeField] private WalkingPlayerController _walkingPlayer;
        [SerializeField] private FlyingPlayerController _flyingPlayer;
        private SelectedPlayer _selectedPlayer = SelectedPlayer.WalkingPlayer;

        protected DisplayManager() { }

        private enum SelectedPlayer
        {
            WalkingPlayer,
            FlyingPlayer
        }

        private void Start()
        {
            SceneManager.sceneLoaded += LevelLoaded;
            LevelLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }

        private void LevelLoaded(Scene scene, LoadSceneMode mode)
        {
            if (_walkingPlayer == null) _walkingPlayer = GameObject.Find("WalkingPlayer").GetComponent<WalkingPlayerController>();
            if (_flyingPlayer == null) _flyingPlayer = GameObject.Find("FlyingPlayer").GetComponent<FlyingPlayerController>();
            if (_walkingCamera == null) _walkingCamera = _walkingPlayer.GetComponentInChildren<Camera>();
            if (_flyingCamera == null) _flyingCamera = _flyingPlayer.GetComponentInChildren<Camera>();
            UpdateSelectedPlayer();
        }

        private void Update()
        {
            if ((_selectedPlayer == SelectedPlayer.WalkingPlayer && _walkingPlayer.AllowControl) || (_selectedPlayer == SelectedPlayer.FlyingPlayer && _flyingPlayer.AllowControl))
            {
                // Hard-code debug toggle player key to tab
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    // Enum hack to toggle selected player (rolls over to WalkingPlayer from FlyingPlayer)
                    _selectedPlayer = (SelectedPlayer)((int)++_selectedPlayer%2);
                    UpdateSelectedPlayer();
                }
            }
        }

        private void UpdateSelectedPlayer()
        {
            if (_selectedPlayer == SelectedPlayer.WalkingPlayer)
            {
                EnableWalkingCamera();
                DisableFlyingCamera();
                EnableWalkingControls();
                DisableFlyingControls();
            }
            else
            {
                EnableFlyingCamera();
                DisableWalkingCamera();
                EnableFlyingControls();
                DisableWalkingControls();
            }
        }

        private void EnableWalkingCamera()
        {
            _walkingCamera.enabled = true;
        }

        private void EnableFlyingCamera()
        {
            _flyingCamera.enabled = true;
        }

        private void DisableWalkingCamera()
        {
            _walkingCamera.enabled = false;
        }

        private void DisableFlyingCamera()
        {
            _flyingCamera.enabled = false;
        }

        private void EnableWalkingControls()
        {
            _walkingPlayer.AllowControl = true;
        }

        private void EnableFlyingControls()
        {
            _flyingPlayer.AllowControl = true;
        }

        private void DisableWalkingControls()
        {
            _walkingPlayer.AllowControl = false;
        }

        private void DisableFlyingControls()
        {
            _flyingPlayer.AllowControl = false;
        }
    }
}