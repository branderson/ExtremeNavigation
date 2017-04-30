using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.Player
{
    public class FlyingMenuController : CustomMonoBehaviour
    {
        private enum MenuState
        {
            None,
            Command
        }

        [SerializeField] private WalkingPlayerController _walkingPlayer;
        [SerializeField] private FlyingPlayerController _flyingPlayer;
        [SerializeField] private FlyingCommandMenuController _commandMenu;
        [SerializeField] private MenuState _state = MenuState.None;
        [SerializeField] private bool _canChangeState = true;

        private void Start()
        {
            switch (_state)
            {
                case MenuState.None:
                    CloseMenu();
                    break;
                case MenuState.Command:
                    OpenCommandMenu();
                    break;
            }
        }

        private void Update()
        {
            if (_canChangeState)
            {
                switch (_state)
                {
                    case MenuState.None:
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            OpenCommandMenu();
                        }
                        break;
                    case MenuState.Command:
                        if (Input.GetKeyDown(KeyCode.Escape))
                        {
                            CloseMenu();
                        }
                        break;
                }
            }
        }

        public void OpenCommandMenu()
        {
            _state = MenuState.Command;
            _walkingPlayer.AllowControl = false;
            _walkingPlayer.LockMovement = true;
            _flyingPlayer.AllowControl = false;
            _flyingPlayer.LockedControl = true;
            _commandMenu.Open();
        }

        public void CloseMenu()
        {
            Cursor.visible = false;
            _state = MenuState.None;
            _walkingPlayer.AllowControl = true;
            _walkingPlayer.LockMovement = false;
            _flyingPlayer.LockedControl = false;
            _flyingPlayer.AllowControl = true;
            _commandMenu.Close();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}