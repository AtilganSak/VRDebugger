using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeatInteractive.VRDebugger
{
    public class ControlButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text countText;
        [SerializeField] private GameObject background;

        public event Action OnClicked;

        private bool _state;
    
        private void OnEnable()
        {
            button.onClick.AddListener(OnClickedButton);
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClickedButton);
        }

        private void Start()
        {
            Refresh();
        }

        public void SetCount(int count)
        {
            countText.text = count.ToString();
        }
    
        private void OnClickedButton()
        {
            OnClicked?.Invoke();
        
            _state = !_state;
        
            Refresh();
        }

        private void Refresh()
        {
            background.SetActive(_state);
        }
    
        public void SetState(bool state)
        {
            _state = state;
        }
    }
}
