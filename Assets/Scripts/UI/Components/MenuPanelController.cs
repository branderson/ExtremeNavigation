using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components
{
	public class MenuPanelController : MonoBehaviour
	{
	    [SerializeField] private bool _hideAtStart = true;
	    [SerializeField] private bool _basePanel = false;
        [SerializeField] private bool _togglePanelAlpha = false;
	    [SerializeField] private GameObject _firstSelected;
	    [SerializeField] protected MenuPanelGroup _menuObjectGroup;          // Group of objects that are displayed while this panel or its child panels are displayed
	    [SerializeField] protected MenuPanelGroup _panelObjectGroup;         // Group of objects that are only displayed while this panel is displayed
        private MenuPanelController _parentPanel;
	    protected Image _image;
	    protected static EventSystem EventSystem;

	    private bool _displayedThisFrame = true;                // Was the panel displayed this frame (necessary to prevent instant close of panels opened with Cancel)
	    private bool _menuActive = true;                       // Is the menu rooted at this panel active

	    protected void Awake()
	    {
            _parentPanel = _basePanel ? null : transform.parent.GetComponent<MenuPanelController>();
	        if (_parentPanel == this) _parentPanel = null;

	        _image = GetComponent<Image>();
	        if (EventSystem == null)
	        {
	            EventSystem = FindObjectOfType<EventSystem>();
	        }
	    }

	    protected void Start()
	    {
            if (!_hideAtStart) return;

		    if (!_basePanel)
		    {
		        Hide();
		    }
		    else
		    {
                Display();
		    }
	    }

		
		protected void Update()
		{
		    if (!_basePanel)
		    {
                if (!_displayedThisFrame && Input.GetButtonDown("Cancel"))
                {
                    Hide();
                }
		    }
		    _displayedThisFrame = false;
		}

        /// <summary>
        /// Displays child panel object group, and selects FirstSelected
        /// </summary>
	    private void DisplayPanel()
        {
            // Don't display if menu not active (this function only for displaying up the hierarchy)
            if (!_menuActive) return;

            // Check for null in case we're in editor
            if (_image == null) _image = GetComponent<Image>();
//            if (_panelObjectGroup == null) _panelObjectGroup = GetComponentInChildren<MenuPanelGroup>();
            if (_togglePanelAlpha)
            {
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 1);
            }
	        _image.raycastTarget = true;

            // Display panel contents
            if (_panelObjectGroup != null)
            {
                _panelObjectGroup.Display();
            }

            if (EventSystem != null)
            {
                EventSystem.SetSelectedGameObject(_firstSelected);
            }
            _displayedThisFrame = true;
	    }

        /// <summary>
        /// Displays child object groups, hide's parent panel object group, and selects FirstSelected
        /// </summary>
	    public void Display()
        {
            _menuActive = true;

            DisplayPanel();

            // Hide parent contents
            if (_parentPanel != null)
            {
                _parentPanel.HidePanel();
            }
            if (_menuObjectGroup != null)
            {
                _menuObjectGroup.Display();
            }
        }

        /// <summary>
        /// Hides child panel object group
        /// </summary>
	    private void HidePanel()
	    {
            if (_image == null) _image = GetComponent<Image>();
//            if (_panelObjectGroup == null) _panelObjectGroup = GetComponentInChildren<MenuPanelGroup>();
            if (_togglePanelAlpha)
            {
                _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);
            }
	        _image.raycastTarget = false;

            if (_panelObjectGroup != null)
            {
                _panelObjectGroup.Hide();
            }
	    }

        /// <summary>
        /// Hides child object groups and displays parent panel, if one exists
        /// </summary>
	    public void Hide()
        {
            // Don't want to hide inactive panels (triggers display on parents)
            if (!_menuActive) return;
            _menuActive = false;

            HidePanel();

            if (_menuObjectGroup != null)
            {
                _menuObjectGroup.Hide();
            }

            if (_parentPanel != null)
            {
                _parentPanel.DisplayPanel();
            }
	    }
	}
}