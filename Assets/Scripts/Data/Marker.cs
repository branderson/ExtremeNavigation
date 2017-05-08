using System.Linq;
using UnityEditorInternal;
using UnityEngine;

namespace Data
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] public bool Active;
        [SerializeField] public bool Start;
        [SerializeField] public bool Middle;
        [SerializeField] public bool End;
        [SerializeField] public Marker Next = null;
        [SerializeField] public Task Task = null;

        private SpriteRenderer _sprite;
        private SpriteRenderer _minimapSprite;

        private void Awake()
        {
            // Get this object's SpriteRenderer
            _sprite = GetComponent<SpriteRenderer>();
            // Get child SpriteRenderer
            _minimapSprite = GetComponentsInChildren<SpriteRenderer>().FirstOrDefault(item => item != _sprite);
            // Disable sprites
            DisableSprite();
            DisableMinimapRecursive();
        }

        /// <summary>
        /// Enable the marker list from here on
        /// </summary>
        public void Enable()
        {
            Active = true;
            EnableSprite();
            EnableMinimapRecursive();
            if (Next != null)
            {
                // Enable list
                Next.Enable();
            }
        }

        /// <summary>
        /// Disable the marker list from here on
        /// </summary>
        public void Disable()
        {
            Active = false;
            DisableSprite();
            DisableMinimapRecursive();
            if (Next != null)
            {
                // Disable list
                Next.Disable();
            }
        }

        public void EnableSprite()
        {
            _sprite.enabled = true;
        }

        public void DisableSprite()
        {
            _sprite.enabled = false;
        }

        public void EnableSpriteRecursive()
        {
            _sprite.enabled = true;
            if (Next != null) Next.EnableSpriteRecursive();
        }

        public void DisableSpriteRecursive()
        {
            _sprite.enabled = false;
            if (Next != null) Next.DisableSpriteRecursive();
        }

        public void EnableMinimapRecursive()
        {
            _minimapSprite.enabled = true;
            if (Next != null) Next.EnableMinimapRecursive();
        }

        public void DisableMinimapRecursive()
        {
            _minimapSprite.enabled = false;
            if (Next != null) Next.DisableMinimapRecursive();
        }

        /// <summary>
        /// Attempt to complete marker
        /// </summary>
        public void RouteEnter()
        {
            // Make sure this marker is active
            if (!Active) return;
            // Make sure this marker is next
            if (!Task.TriggerMarker(this)) return;
            // If this is the end, we've completed the task
            if (Next == null) Task.TaskComplete();
            // Otherwise, move down the chain
            else Task.Head = Next;
            // Trigger this marker
            DisableSprite();
        }
    }
}