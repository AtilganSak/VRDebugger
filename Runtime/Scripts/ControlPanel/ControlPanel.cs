using System;
using UnityEngine;

namespace HeatInteractive.VRDebugger
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private WindowPanel windowPanel;
        [SerializeField] private TabButton[] tabButtons;

        private int _previousSelectedTabIndex = -1;
        
        private void Awake()
        {
            foreach (var tabButton in tabButtons)
            {
                tabButton.OnClick += OnTabButtonClicked;
            }
        }

        private void OnTabButtonClicked(int tabIndex)
        {
            if (tabIndex == _previousSelectedTabIndex)
            {
                tabButtons[tabIndex].Deselect();
                windowPanel.HideWindow(tabButtons[tabIndex].WindowIndex);
                _previousSelectedTabIndex = -1;
                return;
            }
            
            if (_previousSelectedTabIndex != -1)
                tabButtons[_previousSelectedTabIndex].Deselect();

            _previousSelectedTabIndex = tabIndex;
            tabButtons[tabIndex].Select();
            
            windowPanel.ShowWindow(tabButtons[tabIndex].WindowIndex);
        }
    }
}