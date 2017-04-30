using UnityEngine;

namespace Controllers
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField] private float _scrollThreshold = .05f;
        private float _moveStep = .375f; // Move 6 pixels per frame at scale 1, 4 at 2, and 3 at 3
//        private float _moveStep = .1875f;
        private Camera _camera;
        private PixelPerfectCamera _ppCamera;
        private float _scrollAccumulator = 0f;
        private int _currentScale = 0;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _ppCamera = GetComponent<PixelPerfectCamera>();
        }

        private void Start()
        {
            _currentScale = _ppCamera.Zoom(0);
            /*
             * Camera needs to be offset by .1f from whole numbers on the Y-axis initially or we 
             * have alignment problems with the pixel grid
             */
             transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y) + .1f, transform.position.z);
        }

        private void Update()
        {
            HandleZoom();

            HandleMovement();
        }

        private void HandleZoom()
        {
            _scrollAccumulator += Input.GetAxis("Mouse ScrollWheel");

            // Zoom if scrolled enough
            if (_scrollAccumulator > _scrollThreshold)
            {
                _currentScale = _ppCamera.Zoom(1);
                _scrollAccumulator = 0;
            }
            else if (_scrollAccumulator < -_scrollThreshold)
            {
                _currentScale = _ppCamera.Zoom(-1);
                _scrollAccumulator = 0;
            }
        }

        private void HandleMovement()
        {
            // Move by the step amount scaled for larger movement when more zoomed out
            if (Input.GetKey(KeyCode.W))
            {
                // Scale by base of 12 with 1 being 6, 2 being 4, and 3 being 3, so add 1 to scale to skip 12
                transform.position += Vector3.up * _moveStep / (_currentScale + 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * _moveStep / (_currentScale + 1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += Vector3.right * _moveStep / (_currentScale + 1);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position += Vector3.left * _moveStep / (_currentScale + 1);
            }
        }
    }
}