using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.Player
{
	public class WalkingPlayerController : CustomMonoBehaviour
	{
	    [SerializeField] private float _moveSpeed = 5;          // Player speed in m/sec

	    private Transform _cameraTransform;
	    private Camera _camera;
	    private CharacterController _controller;
	    private SmoothMouseLook _mouseLook;

        // Development
	    private bool _allowControl = true;

	    public bool LockMovement = false;

	    public Transform CameraTransform
	    {
	        get { return _cameraTransform; }
	    }

        /// <summary>
        /// Whether the VR player can be controlled
        /// </summary>
	    public bool AllowControl
	    {
            get { return _allowControl; }
            set
            {
                _allowControl = value;
                _mouseLook.enabled = _allowControl;
            }
	    }

	    private void Awake()
	    {
	        _camera = GetComponentInChildren<Camera>();
	        _cameraTransform = _camera.transform;
	        _controller = GetComponent<CharacterController>();
	        _mouseLook = GetComponentInChildren<SmoothMouseLook>();
	    }

	    private void Update()
	    {
	        if (!_allowControl) return;

            // Get input
	        float hor = 0;
	        float ver = 0;
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");
            _mouseLook.enabled = true;

	        if (Input.GetKeyDown(KeyCode.Space))
	        {
	            Collider[] cols = Physics.OverlapSphere(transform.position, .5f);
	            foreach (Collider col in cols)
	            {
	                col.SendMessageUpwards("Interact", SendMessageOptions.DontRequireReceiver);
	            }
	        }

            // Rotate movement vector by VR camera's y rotation
            Vector3 move = new Vector3(hor, 0, ver);
	        move = Quaternion.Euler(0, _cameraTransform.localRotation.eulerAngles.y, 0) * transform.rotation * move;
	        move.y = 0;

//            transform.AdjustLocalPosition(_moveSpeed*move.x*Time.deltaTime, 0, _moveSpeed*move.z*Time.deltaTime);
	        if (!LockMovement)
	        {
                _controller.SimpleMove(move*_moveSpeed);
	        }
	    }
	}
} 