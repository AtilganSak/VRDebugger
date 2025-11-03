using System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace HeatInteractive.VRDebugger
{
    public class VRDebuggerController : MonoBehaviour
    {
        private void Awake()
        {
#if !UNITY_EDITOR
        gameObject.SetActive(Debug.isDebugBuild);
#endif
        }
    }
}
