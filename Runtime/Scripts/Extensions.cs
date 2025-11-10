using System;
using HeatInteractive.VRDebugger;
using UnityEngine;

public static class Extensions
{
    public static VRLogType ToVRLogType(this LogType logType)
    {
        switch (logType)
        {
            case LogType.Error:
                return VRLogType.Error;
            case LogType.Assert:
                return VRLogType.Error;
            case LogType.Warning:
                return VRLogType.Warning;
            case LogType.Log:
                return VRLogType.Info;
            case LogType.Exception:
                return VRLogType.Error;
            default:
                return VRLogType.Info;
        }
    }
}
