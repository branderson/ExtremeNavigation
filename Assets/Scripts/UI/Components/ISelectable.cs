using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI.Components
{
    [System.Serializable]
    public class SelectableEvent : UnityEvent<ISelectable> { }

    public interface ISelectable : IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Gets whether or not the object is selected
        /// </summary>
        /// <returns>
        /// Whether or not the object is selected
        /// </returns>
        bool GetSelected();

        void Select();

        void Deselect();

        new void OnPointerClick(PointerEventData pointer);

        new void OnPointerEnter(PointerEventData pointer);

        new void OnPointerExit(PointerEventData pointer);

//        new void OnPointerDown(PointerEventData pointer);

//        new void OnPointerUp(PointerEventData pointer);

        string GetText();
    }
}