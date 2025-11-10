using TMPro;
using UnityEngine;

namespace HeatInteractive.VRDebugger
{
    public class TabButton_Stats : TabButton_Console
    {
        [SerializeField] private TMP_Text fpsText;
    
        private float _deltaTime;
    
        private void Update()
        {
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            float fps = 1.0f / _deltaTime;
            fpsText.text = Mathf.FloorToInt(fps).ToString();
        }
    }
}
