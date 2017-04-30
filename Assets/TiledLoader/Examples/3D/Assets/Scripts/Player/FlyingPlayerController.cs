using TiledLoader.Examples._3D.Player.Terminal;
using TiledLoader.Examples._3D.Utility.BaseClasses;
using UnityEngine;

namespace TiledLoader.Examples._3D.Player
{
	public class FlyingPlayerController : CustomMonoBehaviour
	{
	    [SerializeField] private float _moveSpeed = 5f;
	    [SerializeField] private float _minimumHeight = 5f;
	    [SerializeField] private float _maximumHeight = 25f;
	    [SerializeField] private bool _canEnable = true;
	    [SerializeField] private TerminalController _terminal;
	    private bool _allowControl = true;

	    private CharacterController _controller;
	    private SmoothMouseLook _mouseLook;

	    public bool LockedControl = false;

        /// <summary>
        /// Whether the monitor player can be controlled
        /// </summary>
	    public bool AllowControl
	    {
            get { return _allowControl; }
            set
            {
                if (!_canEnable) return;
                if (LockedControl) return;
                _allowControl = value;
                if (_allowControl)
                {
                    _mouseLook.enabled = true;
                }
                else
                {
                    _mouseLook.enabled = false;
                }
            }
	    }

	    public void AddPasscode(int id, string code)
	    {
            _terminal.AddPasscode(id, code);
	    }

	    public void AddScriptInfo(string text)
	    {
	        _terminal.AddScriptInfo(text);
	    }

	    private void Awake()
	    {
	        _controller = GetComponent<CharacterController>();
	        _mouseLook = GetComponent<SmoothMouseLook>();
	        if (!_canEnable)
	        {
	            _allowControl = false;
	        }
	    }

	    private void Update()
	    {
	        if (!_allowControl) return;

	        float hor = Input.GetAxis("Horizontal");
	        float ver = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(hor*_moveSpeed, 0, ver*_moveSpeed) * Time.deltaTime;
	        move = transform.rotation * move;

            // Constrain position along Y-axis
	        if (transform.position.y + move.y > _maximumHeight) move.y = _maximumHeight - transform.position.y;
	        else if (transform.position.y + move.y < _minimumHeight) move.y = _minimumHeight - transform.position.y;

	        _controller.Move(move);
	    }
	}
} 