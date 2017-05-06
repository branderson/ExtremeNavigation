using UnityEngine;

namespace Controllers
{
    public class MinimapCameraController : MonoBehaviour
    {
        private LevelController _levelController;
        private Camera _camera;

        private void Awake()
        {
            _levelController = GameObject.FindObjectOfType<LevelController>();
            _camera = GetComponent<Camera>();
        }

        private void Start()
        {
            // Set position and orthographic size to contain entire map
            Rect levelBounds = _levelController.Bounds;
            transform.position = new Vector3(levelBounds.center.x, levelBounds.center.y, transform.position.z);

            _camera.orthographicSize = levelBounds.width / 2;
        }
    }
}