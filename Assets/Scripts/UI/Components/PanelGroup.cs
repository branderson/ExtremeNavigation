using System.Collections.Generic;
using UnityEngine;

namespace UI.Components
{
    public class PanelGroup : MonoBehaviour
    {
        [SerializeField] protected Transform _buttonGroup;
        [SerializeField] protected bool _hideSelectorsOnPanelOpen = true;
        protected List<PanelSelector> _buttons;

        public virtual void Awake()
        {
            InitializeButtons();
        }

        public virtual void Start()
        {
            if (_buttons == null) Debug.Log("[PanelGroup] Start before Awake()");
            CloseOpenPanels();
        }

        private void InitializeButtons()
        {
            _buttons = new List<PanelSelector>();
            
            foreach (Transform child in _buttonGroup)
            {
                PanelSelector button = child.GetComponent<PanelSelector>();
                if (button == null) continue;
                _buttons.Add(button);
                button.Initialize();
            }
        }

        /// <summary>
        /// Hide panel selectors if enabled
        /// </summary>
        public void OpenedPanel()
        {
            if (_hideSelectorsOnPanelOpen)
            {
                _buttonGroup.gameObject.SetActive(false);
            }
        }
            
        ///symotion-linebackward) <summary>
        /// Close any open panels
        /// </summary>
        public void CloseOpenPanels()
        {
            if (_hideSelectorsOnPanelOpen)
            {
                _buttonGroup.gameObject.SetActive(true);
            }
            // TODO: This is kinda-sort bad, figure out why _buttons isn't null in the editor
            if (_buttons == null || _buttons.Count == 0) InitializeButtons();
            foreach (PanelSelector button in _buttons)
            {
                button.ClosePanel();
            }
        }

        public virtual void OnDisable()
        {
            CloseOpenPanels();
        }
    }
}