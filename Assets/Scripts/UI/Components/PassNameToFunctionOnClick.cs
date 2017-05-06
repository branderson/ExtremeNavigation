using UnityEngine;
using Utility;

namespace UI.Components
{
    /// <summary>
    /// Passes name to specified event when clicked
    /// </summary>
	public class PassNameToFunctionOnClick : MonoBehaviour
    {
	    [SerializeField] private Events.StringEvent _function;

	    public void OnClick()
	    {
	        if (_function != null)
	        {
                _function.Invoke(name);
	        }
	    }

        /// <summary>
        /// Sets event to call on click to given event
        /// </summary>
        /// <param name="function">
        /// Event to call when clicked
        /// </param>
	    public void SetFunction(Events.StringEvent function)
	    {
	        _function = function;
	    }
	}
}