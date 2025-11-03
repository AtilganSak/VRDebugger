using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace HeatDebugger.VRDebugger
{
    public class VRD_AppVersion : MonoBehaviour
    {
        [SerializeField] private TMP_Text version_Text;

        private void Awake()
        {
            // var bundleVersion = PlayerSettings.bundleVersion;
            // var appVersion = Application.version;
            // version_Text.text = $"{appVersion}.{bundleVersion}";
        }
    }
}
