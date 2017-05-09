using Controllers;
using UnityEngine;

namespace UI
{
    public class UIPanelController : MonoBehaviour
    {
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private Camera _uiCamera;
        [SerializeField] private float _slideSpeed = 5f;
        private bool _open = true;
        private static float _openPosition = 0;
        private static float _closedPosition = 352;

        private Vector3 _goalPosition;
        private float _goalGameWidth = .6f;

        private void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, _goalPosition, _slideSpeed * Time.deltaTime);
            float gameWidth = _gameCamera.rect.width;
            float newGameWidth = Mathf.Lerp(gameWidth, _goalGameWidth, _slideSpeed * Time.deltaTime);
            _gameCamera.rect = new Rect(0, 0, newGameWidth, 1);
//            _uiCamera.rect = new Rect(newGameWidth, 0, 1 - newGameWidth, 1);
        }

        public void TogglePanel()
        {
            if (_open) ClosePanel();
            else OpenPanel();
        }

        public void RunGame()
        {
            FindObjectOfType<LevelController>().RunPath();
        }

        private void OpenPanel()
        {
            _open = true;
            _goalPosition = new Vector3(_openPosition, 0, 0);
            _goalGameWidth = .6f;
//            _gameCamera.rect = new Rect(0, 0, .6f, 1);
//            _uiCamera.rect = new Rect(.6f, 0, .4f, 1);
        }

        private void ClosePanel()
        {
            _open = false;
            _goalPosition = new Vector3(_closedPosition, 0, 0);
            _goalGameWidth = .967f;
//            _gameCamera.rect = new Rect(0, 0, .967f, 1);
//            _uiCamera.rect = new Rect(.967f, 0, .033f, 1);
        }
    }
}