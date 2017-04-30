using UnityEngine;

namespace Controllers
{
    public class PixelPerfectCamera : MonoBehaviour
    {
        [SerializeField] private int _pixelsPerUnit = 32;
        [SerializeField] private int _defaultScale = 1;
        [SerializeField] private int _maxScale = 5;
        private int _currentScale = 1;
        private Camera _camera;

        private void Awake()
        {
            // Get a reference to the camera
            _camera = GetComponent<Camera>();
            if (_camera == null) DestroyImmediate(this);
            // Ensure camera is orthographic
            _camera.orthographic = true;

            // Set orthographic size to a pixel perfect value
            Resize(_defaultScale);
        }

        /// <summary>
        /// Change the camera's scale by the given amount
        /// </summary>
        /// <param name="amount">
        /// Amount to change the camera's scale. Positive zooms in, negative zooms out
        /// </param>
        /// <returns>
        /// Pixels per unit scale that was zoomed to
        /// </returns>
        public int Zoom(int amount)
        {
            return Resize(_currentScale + amount);
        }

        /// <summary>
        /// Resize the camera's orthographic size to the given PPU scale, while maintaining 
        /// pixel perfect rendering
        /// </summary>
        /// <param name="ppuScale">
        /// Scale of pixels per unit to resize to
        /// </param>
        /// <returns>
        /// Pixels per unit scale that was resized to
        /// </returns>
        public int Resize(int ppuScale)
        {
            // Ensure scale is within bounds
            if (ppuScale <= 0)
            {
                return 1;
            }
            if (ppuScale > _maxScale)
            {
                return _maxScale;
            }
            // Set orthographic size to maintain pixel - physical pixel integer ratio
            _currentScale = ppuScale;
            float verticalResolution = Display.main.renderingHeight;
            float orthoSize = (verticalResolution / (2 * ppuScale * _pixelsPerUnit));
            _camera.orthographicSize = orthoSize;
            return ppuScale;
        }
    }
}