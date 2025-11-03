using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Text;

namespace HeatInteractive.VRDebugger
{
    public static class VRDebug
    {
        public static Dictionary<int, VRDebugLog> DebugLogsDict = new();
        public static Dictionary<int, VRDebugLogCategory> DebugLogCategoriesDict = new();

        private static event Action<int> _onDebugLogged;
        
        public static void Log(string message, VRLogType vrLogType = VRLogType.Info, string category = "", bool expanded = true, bool collapsed = true)
        {
            if (string.IsNullOrEmpty(message))
                message = "Null";
            if (string.IsNullOrEmpty(category))
                category = "Default";

            var uniqueKey = GenerateId(message, vrLogType.ToString(), category);
            
            if (DebugLogsDict.TryGetValue(uniqueKey, out VRDebugLog log))
            {
                log.LogCount++;
            }
            else
            {
                DebugLogsDict.Add(uniqueKey, new VRDebugLog(message, vrLogType,1));
                DebugLogCategoriesDict.Add(uniqueKey, new VRDebugLogCategory(category, expanded, collapsed));
            }
            
            _onDebugLogged?.Invoke(uniqueKey);
        }

        public static void SubscribeDebugLogEvent(Action<int> callback)
        {
            _onDebugLogged += callback;
        }
        public static void UnsubscribeDebugLogEvent(Action<int> callback)
        {
            _onDebugLogged -= callback;
        }
        private static int GenerateId(string message, string logType, string category)
        {
            string combined = $"{message}:{logType}:{category}";
            byte[] data = Encoding.UTF8.GetBytes(combined);
            uint hash = Crc32.HashToUInt32(data);
            return unchecked((int)hash);
        }
    }

    [Serializable]
    public struct VRDebugLog
    {
        public string Message;
        public VRLogType vrLogType;
        public int LogCount;

        public VRDebugLog(string message, VRLogType vrLogType = VRLogType.Info, int logCount = 0)
        {
            Message = message;
            this.vrLogType = vrLogType;
            LogCount = logCount;
        }
    }
    [Serializable]
    public struct VRDebugLogCategory
    {
        public string CategoryName;
        public bool Expanded;
        public bool Collapsed;

        public VRDebugLogCategory(string categoryName, bool expanded = false, bool collapsed = false)
        {
            CategoryName = categoryName;
            Expanded = expanded;
            Collapsed = collapsed;
        }
    }

    public enum VRLogType
    {
        Info,
        Warning,
        Error
    }
}