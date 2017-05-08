using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Components
{
    /// <summary>
    /// Controls a tabbed group of panels
    /// </summary>
    public class TabbedPanelGroup : PanelGroup
    {
        [SerializeField] private GameObject _panelSelectorPrefab;
        [SerializeField] private GameObject _titlePrefab;
        [SerializeField] private UnityEvent _tabSwitchFunction;
         
        public override void Awake()
        {
            _buttons = new List<PanelSelector>();
            _hideSelectorsOnPanelOpen = false;

            if (_titlePrefab != null)
            {
                GameObject title = Instantiate(_titlePrefab);
                title.transform.SetParent(_buttonGroup, false);
                Text titleText = title.GetComponentInChildren<Text>();
                titleText.text = transform.parent.name;
            }

            foreach (Transform child in transform)
            {
                MenuPanelController panelController = child.GetComponent<MenuPanelController>();
                if (panelController == null) continue;
                PanelSelector button = Instantiate(_panelSelectorPrefab).GetComponent<PanelSelector>();
                button.transform.SetParent(_buttonGroup, false);
                _buttons.Add(button);
                button.Initialize(panelController);
                button.OpenPanelFunction = _tabSwitchFunction;
                button.Panel = panelController;
                button.PanelGroup = this;
            }
        }

        public override void Start()
        {
            base.Start();
            PanelSelector firstTab = _buttons.FirstOrDefault();
            if (firstTab != null)
            {
                firstTab.OpenPanel();
            }
        }

        public override void OnDisable()
        {
            PanelSelector firstButton = _buttons.FirstOrDefault();
            CloseOpenPanels();
            if (firstButton != null)
            {
                firstButton.OpenPanel();
            }
        }
    }
}