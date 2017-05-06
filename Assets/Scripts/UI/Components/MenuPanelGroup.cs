using UnityEngine;

namespace UI.Components
{
    /// <summary>
    /// Handles hiding and displaying child objects
    /// </summary>
	public class MenuPanelGroup : MonoBehaviour
	{
        /// <summary>
        /// Deactivates all child objects
        /// </summary>
	    public void Hide()
	    {
	        for (int i = 0; i < transform.childCount; i++)
	        {
	            transform.GetChild(i).gameObject.SetActive(false);
	        }
	    }

        /// <summary>
        /// Activates all child objects
        /// </summary>
	    public void Display()
	    {
	        for (int i = 0; i < transform.childCount; i++)
	        {
	            transform.GetChild(i).gameObject.SetActive(true);
	        }
	    }
	}
}