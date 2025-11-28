using System;
using UnityEngine;
using UnityEngine.UI;

namespace HeatInteractive.VRDebugger
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField] private int tabIndex;
        [SerializeField] private int windowIndex;
        [SerializeField] private Button button;
        [SerializeField] private Image bg;

        public int WindowIndex => windowIndex;
        
        public event Action<int> OnClick;
        
        protected virtual void Awake()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            OnClick?.Invoke(tabIndex);
        }

        public void Select()
        {
            bg.enabled = true;
        }

        public void Deselect()
        {
            bg.enabled = false;
        }
    }
}
