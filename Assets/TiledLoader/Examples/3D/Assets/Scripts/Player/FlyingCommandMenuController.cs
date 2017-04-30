using TiledLoader.Examples._3D.Utility.BaseClasses;

namespace TiledLoader.Examples._3D.Player
{
    public class FlyingCommandMenuController : CustomMonoBehaviour, IMenu
    {
        private MenuPanelController _panel;

        private void Awake()
        {
            _panel = GetComponentInChildren<MenuPanelController>();
        }

        public void Open()
        {
            _panel.gameObject.SetActive(true);
        }

        public void Close()
        {
            _panel.gameObject.SetActive(false);
        }
    }
}