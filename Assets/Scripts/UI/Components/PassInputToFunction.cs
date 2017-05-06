using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Components
{
    /// <summary>
    /// Component to pass input text to a function
    /// </summary>
	public class PassInputToFunction : MonoBehaviour
	{
	    [SerializeField] private Events.StringEvent _function; // Function must be a dynamic call for this to work
	    [SerializeField] private InputField _input;

        /// <summary>
        /// Sends text in input to function
        /// </summary>
	    public void SendText()
	    {
	        _function.Invoke(_input.text);
	    }
	}
}