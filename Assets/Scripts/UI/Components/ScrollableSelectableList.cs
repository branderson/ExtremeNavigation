using System.Collections.Generic;
using System.Linq;
using TiledLoader;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    /// <summary>
    /// Controller which handles scrollable lists of selectable options
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
	public class ScrollableSelectableList : MonoBehaviour
    {
        [SerializeField] private GameObject _optionTemplate;
        [SerializeField] private SelectableEvent _selectFunction;
	    [SerializeField] private List<GameObject> _options;
        [SerializeField] private ScrollRect _scrollRect;

        /// <summary>
        /// Get a list of names of options in the list
        /// </summary>
        public List<string> Options
        {
            get { return _options.Select(item => item.name).ToList(); }
        }

        /// <summary>
        /// Get a list of names of options in the list
        /// </summary>
        public List<ISelectable> OptionObjects
        {
            get { return _options.Select(item => item.GetComponent<ISelectable>()).ToList(); }
        }

        private ScrollRect Scroll
        {
            get
            {
                if (_scrollRect == null)
                {
                    _scrollRect = GetComponent<ScrollRect>();
                    _scrollRect.viewport.GetComponentInChildren<GridLayoutGroup>().cellSize = 
                        new Vector2(_scrollRect.GetComponent<RectTransform>().rect.width - 
                        _scrollRect.verticalScrollbar.GetComponent<RectTransform>().rect.width,
                        _optionTemplate.GetComponent<RectTransform>().rect.height);
                }
                return _scrollRect;
            }
        }

        /// <summary>
        /// Populates the scroll list with instances of the option template whose ListOption components 
        /// match the given list
        /// </summary>
        /// <param name="options">
        /// List of ListOptions to assign to instantiated options
        /// </param>
	    public void Populate(List<TiledLoaderPropertyData> options)
	    {
            // TODO: Return ListOption from get calls
            if (_options == null) _options = new List<GameObject>();
            Depopulate();
	        foreach (TiledLoaderPropertyData option in options)
	        {
                GameObject optionObject = Instantiate(_optionTemplate) as GameObject;
                optionObject.SetActive(true);
                optionObject.GetComponent<ListOption>().Configure(option);
	            optionObject.GetComponent<SelectableButton>().SetFunction(_selectFunction);
	            RectTransform optionRect = optionObject.GetComponent<RectTransform>();
                optionRect.SetParent(Scroll.content, false);
                _options.Add(optionObject);
	        }
	    }

        /// <summary>
        /// Destroys all options from scroll list
        /// </summary>
        public void Depopulate()
        {
            if (_options == null) _options = new List<GameObject>();
            // TODO: Why doesn't iterating through children work?
//            foreach (Transform option in Scroll.content.transform)
            foreach (GameObject option in _options)
            {
                DestroyImmediate(option, true);
            }
            _options.Clear();
        }

        /// <summary>
        /// Gets a list of all selected options
        /// </summary>
        /// <returns>
        /// List of all selected options
        /// </returns>
        public List<ISelectable> GetSelected()
        {
            List<ISelectable> selected = new List<ISelectable>();

            foreach (GameObject option in _options)
            {
                ISelectable selectable = option.GetComponent<ISelectable>();

                if (selectable.GetSelected())
                {
                    selected.Add(selectable);
                }
            }
            return selected;
        }

//        public void OnDisable()
//        {
//            Depopulate();
//        }
	}
}