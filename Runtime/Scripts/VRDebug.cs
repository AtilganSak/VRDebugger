using System;
using System.Collections.Generic;
using System.IO.Hashing;
using System.Text;

namespace HeatInteractive.VRDebugger
{
    public static class VRDebug
    {
        public static Dictionary<int, VRDebugLog> DebugLogsDict = new();
        public static Dictionary<int, VRDebugLogCategory> LogCategoriesDict = new();
        public static Dictionary<int, VRConstantLog> ConstantLogsDict = new();

        private static event Action<int> _onDebugLogged;
        private static event Action<int> _onConstantLogged;

        public static void Log(string message, VRLogType vrLogType = VRLogType.Info, string category = "", bool expanded = true, bool collapsed = true)
        {
            if (string.IsNullOrEmpty(message))
                message = "Null";
            if (string.IsNullOrEmpty(category))
                category = "Default";

            var uniqueKey = GenerateDebugLogId(message, vrLogType.ToString(), category);

            if (DebugLogsDict.TryGetValue(uniqueKey, out VRDebugLog log))
            {
                log.LogCount++;
            }
            else
            {
                DebugLogsDict.Add(uniqueKey, new VRDebugLog(message, vrLogType, 1));
                LogCategoriesDict.Add(uniqueKey, new VRDebugLogCategory(category, expanded, collapsed));
            }

            _onDebugLogged?.Invoke(uniqueKey);
        }

        public static void LogConstant(string constantName, object value, string category = "", bool expanded = true)
        {
            if (string.IsNullOrEmpty(constantName))
                constantName = "Null";
            if (string.IsNullOrEmpty(category))
                category = "Default";

            var uniqueKey = GenerateConstantLogId(constantName, category);
            
            if (ConstantLogsDict.ContainsKey(uniqueKey))
            {
                VRConstantLog log = ConstantLogsDict[uniqueKey];
                log.Value = value;
                ConstantLogsDict[uniqueKey] = log;
            }
            else
            {
                ConstantLogsDict.Add(uniqueKey, new VRConstantLog(constantName, value));
                LogCategoriesDict.Add(uniqueKey, new VRDebugLogCategory(category, expanded, false));
            }

            _onConstantLogged?.Invoke(uniqueKey);
        }

        public static void SubscribeDebugLogEvent(Action<int> callback)
        {
            _onDebugLogged += callback;
        }

        public static void UnsubscribeDebugLogEvent(Action<int> callback)
        {
            _onDebugLogged -= callback;
        }

        public static void SubscribeConstantLogEvent(Action<int> callback)
        {
            _onConstantLogged += callback;
        }

        public static void UnsubscribeConstantLogEvent(Action<int> callback)
        {
            _onConstantLogged -= callback;
        }

        private static int GenerateDebugLogId(string message, string logType, string category)
        {
            string combined = $"{message}:{logType}:{category}";
            byte[] data = Encoding.UTF8.GetBytes(combined);
            uint hash = Crc32.HashToUInt32(data);
            return unchecked((int)hash);
        }

        private static int GenerateConstantLogId(string constantName, string category)
        {
            string combined = $"{constantName}:{category}";
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
    public struct VRConstantLog
    {
        public string ConstantName;
        public object Value;

        public VRConstantLog(string constantName, object value)
        {
            ConstantName = constantName;
            Value = value;
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