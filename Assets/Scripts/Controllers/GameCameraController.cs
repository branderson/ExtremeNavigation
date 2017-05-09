using UnityEngine;

namespace Controllers
{
    public class GameCameraController : MonoBehaviour
    {
        [SerializeField] private float _scrollThreshold = .05f;
        [SerializeField] private float _edgeScrollSpeed = .375f;

        private float _moveStep = .375f; // Move 6 pixels per frame at scale 1, 4 at 2, and 3 at 3
//        private float _moveStep = .1875f;

        private Camera _camera;
        private PixelPerfectCamera _ppCamera;
        private LevelController _levelController;

        private float _scrollAccumulator = 0f;
        private int _currentScale = 0;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _ppCamera = GetComponent<PixelPerfectCamera>();
            _levelController = GameObject.FindObjectOfType<LevelController>();
        }

        private void Start()
        {
            _currentScale = _ppCamera.Zoom(0);
            /*
             * Camera needs to be offset by .1f from whole numbers on the Y-axis initially or we 
             * have alignment problems with the pixel grid
             */
//             transform.position = new Vector3(transform.position.x, Mathf.Floor(transform.position.y) + .1f, transform.position.z);
//            transform.position += new Vector3(0, .1f, 0);
        }

        private void Update()
        {
            HandleZoom();

            HandleMovement();
        }

        private void HandleZoom()
        {
            // Don't zoom if on planner
            if (Input.mousePosition.x <= _camera.pixelWidth)
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

            // Prevent zooming which would display larger than map
            float cameraExtentY = _camera.orthographicSize;
            float cameraExtentX = cameraExtentY * _camera.pixelWidth / _camera.pixelHeight;
            while (cameraExtentX > _levelController.Bounds.width / 2f || cameraExtentY > _levelController.Bounds.height / 2f)
            {
                _currentScale = _ppCamera.Zoom(1);
                cameraExtentY = _camera.orthographicSize;
                cameraExtentX = cameraExtentY * _camera.pixelWidth / _camera.pixelHeight;
            }
        }

        private void HandleMovement()
        {
            // Get position
            Vector3 pos = transform.position;
            Vector2 edgeScroll = EdgeScroll();

            // Move by the step amount scaled for larger movement when more zoomed out
            if (Input.GetKey(KeyCode.W))
            {
                // Scale by base of 12 with 1 being 6, 2 being 4, and 3 being 3, so add 1 to scale to skip 12
                pos += Vector3.up * _moveStep / (_currentScale + 1);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                pos += Vector3.down * _moveStep / (_currentScale + 1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                pos += Vector3.right * _moveStep / (_currentScale + 1);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                pos += Vector3.left * _moveStep / (_currentScale + 1);
            }
            pos.x += edgeScroll.x / (_currentScale + 1);
            pos.y += edgeScroll.y / (_currentScale + 1);

            // Constrain camera viewport to map
            float cameraExtentY = _camera.orthographicSize;
            float cameraExtentX = cameraExtentY * _camera.pixelWidth / _camera.pixelHeight;
            float minX = cameraExtentX + _levelController.Bounds.xMin;
            float maxX = -cameraExtentX + _levelController.Bounds.xMax;
            float minY = cameraExtentY + _levelController.Bounds.yMin;
            float maxY = -cameraExtentY + _levelController.Bounds.yMax;
            if (pos.x > maxX)
            {
                pos.x = maxX;
            }
            else if (pos.x < minX)
            {
                pos.x = minX;
            }
            if (pos.y > maxY)
            {
                pos.y = maxY;
            }
            else if (pos.y < minY)
            {
                pos.y = minY;
            }
            transform.position = pos;
        }

        private Vector2 EdgeScroll()
        {
            float hor = 0;
            float ver = 0;
            // Find size of the camera assuming starting at left full size
            float cameraWidth = _camera.pixelWidth;
            float cameraHeight = _camera.pixelHeight;

            // Edge movement
		    if (_edgeScrollSpeed > 0)
		    {
                if (Input.mousePosition.x < cameraWidth/10f)
                {
                    float val = _edgeScrollSpeed * (Input.mousePosition.x - cameraWidth/10f) / (.1f * cameraWidth);
                    if (val < hor)
                    {
//                        hor = val;
                        hor = -_edgeScrollSpeed;
                    }
                }
                else if (Input.mousePosition.x > 9*cameraWidth/10f && Input.mousePosition.x < cameraWidth)
                {
                    float val = _edgeScrollSpeed * (Input.mousePosition.x - 9*cameraWidth/10f)/(.1f*cameraWidth);
                    if (val > hor)
                    {
//                        hor = val;
                        hor = _edgeScrollSpeed;
                    }
                }
                if (Input.mousePosition.y < cameraHeight/5f && Input.mousePosition.x < cameraWidth)
                {
                    float val = _edgeScrollSpeed * (Input.mousePosition.y - cameraHeight/5f) / (cameraHeight * 1/5f);
                    if (val < ver)
                    {
//                        ver = val;
                        ver = -_edgeScrollSpeed;
                    }
                }
                else if (Input.mousePosition.y > 4*cameraHeight/5f && Input.mousePosition.x < cameraWidth)
                {
                    float val = _edgeScrollSpeed * (Input.mousePosition.y - 4*cameraHeight/5f)/(cameraHeight * 1/5f);
                    if (val > ver)
                    {
//                        ver = val;
                        ver = _edgeScrollSpeed;
                    }
                }
		    }

            return new Vector2(hor, ver);
        }
    }
}