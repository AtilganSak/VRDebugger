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
        
        public void Init(int uniqueID, Sprite logTypeIcon)
        {
            messageText.text = VRDebug.DebugLogsDict[uniqueID].Message;
            logTypeImage.sprite = logTypeIcon;

            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        public void Refresh(int uniqueID)
        {
            logCountObject.SetActive(VRDebug.DebugLogsDict[uniqueID].LogCount > 0);
            logCountText.text = VRDebug.DebugLogsDict[uniqueID].LogCount.ToString();
        }
    }
}