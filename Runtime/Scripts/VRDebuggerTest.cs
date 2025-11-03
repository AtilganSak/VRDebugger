using HeatInteractive.VRDebugger;
using Sirenix.OdinInspector;
using UnityEngine;
using LogType = UnityEngine.LogType;

public class VRDebuggerTest : MonoBehaviour
{
    public string mssage;
    public VRLogType logType;
    public string categoryName;
    public bool collapsed;
    
    [Button]
    private void Log()
    {
        VRDebug.Log(mssage, logType, categoryName, true,collapsed);
    }
}
