using System.Linq;
using UnityEngine;

namespace Data
{
    public class Marker : MonoBehaviour
    {
//        /// <summary>
//        /// Actions to change marker state
//        /// </summary>
//        public enum MarkerAction
//        {
//            Select,
//            Deselect,
//            Activate,
//            Deactivate,
//            Complete,
//        }

        /// <summary>
        /// States a marker can be in
        /// </summary>
        private enum MarkerState
        {
            Inactive,
            InactiveHighlighted,
            Active,
            ActiveHightlighted,
            Complete,
            CompleteHighlighted,
        }

        [SerializeField] public bool First;
        [SerializeField] public bool Middle;
        [SerializeField] public bool End;
        [SerializeField] public Marker Next = null;
        [SerializeField] public Task Task = null;

//        private bool _active;
        private MarkerState _state;

        private SpriteRenderer _sprite;
        private SpriteRenderer _minimapSprite;
        private SpriteRenderer _highlightedSprite;
        private SpriteRenderer _highlightedMinimapSprite;
        private TextMesh _valueText;

        private void Awake()
        {
            // Get this object's SpriteRenderer
            _sprite = GetComponent<SpriteRenderer>();
            // Get child SpriteRenderers
            _minimapSprite = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(item => item.name == "MinimapTile");
            _highlightedSprite = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(item => item.name == "HighlightedSprite");
            _highlightedMinimapSprite = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(item => item.name == "HighlightedMinimapSprite");
            // Get text renderer
            _valueText = GetComponentInChildren<TextMesh>();
            _valueText.text = Task.Name + "\n$" + Task.Value.ToString();
        }

        public void Select()
        {
            if (Next != null) Next.Select();
            if (_state == MarkerState.Inactive) SetState(MarkerState.InactiveHighlighted);
            else if (_state == MarkerState.Active) SetState(MarkerState.ActiveHightlighted);
            else if (_state == MarkerState.Complete) SetState(MarkerState.CompleteHighlighted);
        }

        public void Deselect()
        {
            if (Next != null) Next.Deselect();
            if (_state == MarkerState.InactiveHighlighted) SetState(MarkerState.Inactive);
            else if (_state == MarkerState.ActiveHightlighted) SetState(MarkerState.Active);
            else if (_state == MarkerState.CompleteHighlighted) SetState(MarkerState.Complete);
        }

        public void Activate()
        {
            if (Next != null) Next.Activate();
            SetState(MarkerState.Active);
        }

        public void Deactivate()
        {
            if (Next != null) Next.Deactivate();
            SetState(MarkerState.Inactive);
        }

        public void Complete()
        {
            SetState(_state == MarkerState.ActiveHightlighted ? MarkerState.CompleteHighlighted : MarkerState.Complete);
        }

        private void SetState(MarkerState state)
        {
//            switch (state)
//            {
//                case MarkerState.Inactive:
////                    DisableRecursive();
//                    break;
//                case MarkerState.InactiveHighlighted:
//                    break;
//                case MarkerState.Active:
////                    EnableRecursive();
//                    break;
//                case MarkerState.ActiveHightlighted:
//                    break;
//                case MarkerState.Complete:
//                    break;
//                case MarkerState.CompleteHighlighted:
//                    break;
//            }
            _state = state;
            SetSpritesRecursive();
        }

        /// <summary>
        /// Enable the marker list from here on
        /// </summary>
        private void EnableRecursive()
        {
        }

        /// <summary>
        /// Disable the marker list from here on
        /// </summary>
        private void DisableRecursive()
        {
        }

        /// <summary>
        /// Set sprites for marker list from here on to appropriate state
        /// </summary>
        private void SetSpritesRecursive()
        {
            switch (_state)
            {
                case MarkerState.Inactive:
                    _sprite.enabled = false;
                    _minimapSprite.enabled = false;
                    _highlightedSprite.enabled = false;
                    _highlightedMinimapSprite.enabled = false;
                    _valueText.gameObject.SetActive(false);
                    break;
                case MarkerState.InactiveHighlighted:
                    _sprite.enabled = true;
                    _minimapSprite.enabled = false;
                    _highlightedSprite.enabled = true;
                    _highlightedMinimapSprite.enabled = true;
                    _valueText.gameObject.SetActive(true);
                    break;
                case MarkerState.Active:
                    _sprite.enabled = true;
                    _minimapSprite.enabled = true;
                    _highlightedSprite.enabled = false;
                    _highlightedMinimapSprite.enabled = false;
                    _valueText.gameObject.SetActive(true);
                    break;
                case MarkerState.ActiveHightlighted:
                    _sprite.enabled = true;
                    _minimapSprite.enabled = false;
                    _highlightedSprite.enabled = true;
                    _highlightedMinimapSprite.enabled = true;
                    _valueText.gameObject.SetActive(true);
                    break;
                case MarkerState.Complete:
                    _sprite.enabled = false;
                    _minimapSprite.enabled = false;
                    _highlightedSprite.enabled = false;
                    _highlightedMinimapSprite.enabled = false;
                    _valueText.gameObject.SetActive(false);
                    break;
                case MarkerState.CompleteHighlighted:
                    _sprite.enabled = true;
                    _minimapSprite.enabled = false;
                    _highlightedSprite.enabled = true;
                    _highlightedMinimapSprite.enabled = true;
                    _valueText.gameObject.SetActive(true);
                    break;
            }
            if (Next != null) Next.SetSpritesRecursive();
        }

        /// <summary>
        /// Attempt to complete marker
        /// </summary>
        public void RouteEnter()
        {
            // Make sure this marker is active
            if (_state != MarkerState.Active && _state != MarkerState.ActiveHightlighted) return;
            // Make sure this marker is next
            if (!Task.TriggerMarker(this)) return;
            // If this is the end, we've completed the task
            if (Next == null) Task.TaskComplete();
            // Otherwise, move down the chain
            else Task.Head = Next;
            // Trigger this marker
            Complete();
        }
    }
}