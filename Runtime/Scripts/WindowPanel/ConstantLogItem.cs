using HeatInteractive.VRDebugger;
using TMPro;
using UnityEngine;

namespace HeatInteractive
{
    public class ConstantLogItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text valueText;
        
        public void Init(string name)
        {
            nameText.text = name;
        }

        public void Refresh(int uniqueID)
        {
            valueText.text = VRDebug.ConstantLogsDict[uniqueID].Value.ToString();
        }
    }
}
