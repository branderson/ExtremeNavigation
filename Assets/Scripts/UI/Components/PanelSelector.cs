using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class PanelSelector : MonoBehaviour
    {
        [SerializeField] private Sprite _selectedBackground;
        [SerializeField] private Sprite _unselectedBackground;
        [SerializeField] private Sprite _hoveredBackground;
        [SerializeField] public MenuPanelController Panel;
        public PanelGroup PanelGroup;
        private bool _selected = false;

        private Image _background;

        public void Initialize()
        {
            _background = GetComponent<Image>();
            _background.sprite = _unselectedBackground;
        }

        /// <summary>
        /// Initialize the selector to control the given panel
        /// </summary>
        /// <param name="panel">
        /// The panel this selector controls
        /// </param>
        public void Initialize(MenuPanelController panel)
        {
            Initialize();
            Panel = panel;
            GetComponentInChildren<Text>().text = panel.name;
        }

        /// <summary>
        /// Display this selector's panel
        /// </summary>
        public void OpenPanel()
        {
            if (Panel == null) return;
            PanelGroup.CloseOpenPanels();
            Panel.Display();
            PanelGroup.OpenedPanel();
            // This could be called in the editor so we need to check for null
            if (_background == null) _background = GetComponent<Image>();
            _background.sprite = _selectedBackground;
            _selected = true;
        }

        /// <summary>
        /// Hide this selector's panel
        /// </summary>
        public void ClosePanel()
        {
            if (Panel == null) return;
            if (_background == null) _background = GetComponent<Image>();
            _background.sprite = _unselectedBackground;
            Panel.Hide();
            _selected = false;
        }

        public void OnMouseEnter()
        {
            if (_hoveredBackground == null) return;
            if (_background.sprite != _unselectedBackground) return;
            _background.sprite = _hoveredBackground;
        }

        public void OnMouseExit()
        {
            if (_hoveredBackground == null) return;
            if (_background.sprite != _hoveredBackground) return;
            _background.sprite = _selected ? _selectedBackground : _unselectedBackground;
        }
    }
}