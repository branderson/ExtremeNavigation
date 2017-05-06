using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Components
{
    public class SelectableButton : Button, ISelectable
    {
        private SelectableEvent _function;
        private bool _selected = false;

        public bool GetSelected()
        {
            return _selected;
        }

        public new void Select()
        {
            _selected = true;
            DoStateTransition(SelectionState.Pressed, true);
        }

        public void Deselect()
        {
            _selected = false;
            DoStateTransition(SelectionState.Normal, true);
        }

        public new void OnPointerClick(PointerEventData pointer)
        {
            if (_function != null)
            {
                _function.Invoke(this);
            }
        }

        public new void OnPointerEnter(PointerEventData pointer) { }

        public new void OnPointerExit(PointerEventData pointer) { }

//	    public new void OnPointerDown(PointerEventData pointer)
//	    {
//	        if (pointer.button == PointerEventData.InputButton.Left)
//	        {
//                // Left mouse click
//                if (_function != null)
//                {
//                    _function.Invoke(this);
//                }
//	        }
//	    }

        public string GetText()
        {
            return name;
        }

        /// <summary>
        /// Sets event to call on click to given event
        /// </summary>
        /// <param name="function">
        /// Event to call when clicked
        /// </param>
	    public void SetFunction(SelectableEvent function)
	    {
	        _function = function;
	    }
    }
}