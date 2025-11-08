using System;
using UnityEditor;
using UnityEngine;

namespace HeatInteractive.VRDebugger
{
    public class WindowPanel : MonoBehaviour
    {
        [SerializeField] private BaseWindow[] windows;

        private int _previousWindowIndex;
        
        private void Awake()
        {
            for (int i = 0; i < windows.Length; i++)
            {
                if(i == 1)
                    windows[i].Show();
                else
                    windows[i].Hide();
            }

            _previousWindowIndex = 0;
        }

        public void ShowWindow(int index)
        {
            windows[_previousWindowIndex].Hide();
            _previousWindowIndex = index;
            if(index >= 0 && index < windows.Length)
                windows[index].Show();
        }

        public void HideWindow(int index)
        {
            if(index >= 0 && index < windows.Length)
                windows[index].Hide();
        }
    }
}
