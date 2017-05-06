using UnityEngine;
using Utility;

namespace UI.Components
{
    /// <summary>
    /// Passes name to specified event when hovered over with the mouse
    /// </summary>
	public class PassNameToFunctionOnHover : MonoBehaviour
	{
	    [SerializeField] private Events.StringEvent _enterFunction;
	    [SerializeField] private Events.StringEvent _exitFunction;

	    public void OnMouseOver()
	    {
	    }

        public void OnMouseEnter()
        {
	        if (_enterFunction != null)
	        {
                _enterFunction.Invoke(name);
	        }
        }

        public void OnMouseExit()
        {
	        if (_exitFunction != null)
	        {
                _exitFunction.Invoke(name);
	        }
        }

        /// <summary>
        /// Sets event to call on hover to given event
        /// </summary>
        /// <param name="function">
        /// Event to call when hovered over
        /// </param>
	    public void SetEnterFunction(Events.StringEvent function)
	    {
	        _enterFunction = function;
	    }

        /// <summary>
        /// Sets event to call on mouse exit to given event
        /// </summary>
        /// <param name="function">
        /// Event to call when mouse leaves object
        /// </param>
	    public void SetExitFunction(Events.StringEvent function)
	    {
	        _exitFunction = function;
	    }
	}
}