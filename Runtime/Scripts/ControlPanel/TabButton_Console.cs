using System;
using TMPro;
using UnityEngine;

namespace HeatInteractive.VRDebugger
{
    public class TabButton_Console : TabButton
    {
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private TMP_Text warningText;
        [SerializeField] private TMP_Text errorText;

        private void Update()
        {
            int infoCount = 0;
            int warningCount = 0;
            int errorCount = 0;
            foreach (var pair in VRDebug.DebugLogsDict)
            {
                if (pair.Value.vrLogType == VRLogType.Info)
                    infoCount += pair.Value.LogCount;
                else if (pair.Value.vrLogType == VRLogType.Warning)
                    warningCount += pair.Value.LogCount;
                else
                    errorCount += pair.Value.LogCount;
            }

            infoText.text = infoCount.ToString();
            warningText.text = warningCount.ToString();
            errorText.text = errorCount.ToString();
        }
    }
}