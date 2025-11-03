using HeatInteractive.VRDebugger;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeatInteractive.VRDebugger
{
    public class DebugLogItem : MonoBehaviour
    {
        [SerializeField] private Image logTypeImage;
        [SerializeField] private TMP_Text messageText;
        [SerializeField] private GameObject logCountObject;
        [SerializeField] private TMP_Text logCountText;
        [SerializeField] private RectTransform rectTransform;

        private int  _uniqueID;
    
        public void Init(int uniqueID, Sprite logTypeIcon)
        {
            _uniqueID = uniqueID;
        
            messageText.text = VRDebug.DebugLogsDict[_uniqueID].Message;
            logTypeImage.sprite = logTypeIcon;

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        public void Refresh()
        {
            logCountObject.SetActive(VRDebug.DebugLogsDict[_uniqueID].LogCount > 0);
            logCountText.text = VRDebug.DebugLogsDict[_uniqueID].LogCount.ToString();
        }
    }
}
