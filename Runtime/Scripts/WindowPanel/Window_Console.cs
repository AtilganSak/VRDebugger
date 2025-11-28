using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeatInteractive.VRDebugger
{
    public class Window_Console : BaseWindow
    {
        [Header("Category Items")]
        [SerializeField] private CategoryItem debugLogCategoryItemPrefab;
        [SerializeField] private CategoryItem constantCategoryItemPrefab;

        [Header("Log Items")]
        [SerializeField] private DebugLogItem debugLogItemPrefab;
        [SerializeField] private ConstantLogItem constantLogItemPrefab;

        [Header("Buttons")]
        [SerializeField] private Button clearButton;

        [SerializeField] private ControlButton infoControlButton;
        [SerializeField] private ControlButton warningControlButton;
        [SerializeField] private ControlButton errorControlButton;

        [Header("Common")]
        [SerializeField] private Transform debugLogsParent;
        [SerializeField] private Transform constantLogsParent;

        [SerializeField] private Sprite infoIcon;
        [SerializeField] private Sprite warningIcon;
        [SerializeField] private Sprite errorIcon;

        private Dictionary<int, List<DebugLogItem>> _debugLogs = new();
        private Dictionary<int, ConstantLogItem> _constantLogs = new();
        private Dictionary<string, CategoryItem> _debugLogCategories = new();
        private Dictionary<string, CategoryItem> _constantLogCategories = new();
        private Dictionary<VRLogType, int> _logCounts = new();
        private Dictionary<VRLogType, bool> _logVisibles = new();

        protected override void Awake()
        {
            base.Awake();

            VRDebug.SubscribeDebugLogEvent(OnNewDebugLogged);
            VRDebug.SubscribeConstantLogEvent(OnNewConstantLogged);
            Application.logMessageReceived += OnNewLogMessageReceived;

            clearButton.onClick.AddListener(OnPressedClearButton);
            infoControlButton.OnClicked += () => OnPressedControlButton(VRLogType.Info);
            warningControlButton.OnClicked+= () => OnPressedControlButton(VRLogType.Warning);
            errorControlButton.OnClicked += () => OnPressedControlButton(VRLogType.Error);

            infoControlButton.SetState(true);
            warningControlButton.SetState(true);
            errorControlButton.SetState(true);
            
            var logTypeList = Enum.GetValues(typeof(VRLogType));
            foreach (var logType in logTypeList)
            {
                _logVisibles.Add((VRLogType)logType, true);
                _logCounts.Add((VRLogType)logType, 0);
            }
        }

        private void OnDestroy()
        {
            VRDebug.UnsubscribeDebugLogEvent(OnNewDebugLogged);
            VRDebug.UnsubscribeConstantLogEvent(OnNewConstantLogged);
        }
        
        private void OnNewLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            VRDebug.Log(condition, type.ToVRLogType(), "Unity Logs");
        }

        private void OnNewDebugLogged(int uniqueID)
        {
            CreateDebugLogCategoryIfNotExists(VRDebug.LogCategoriesDict[uniqueID].CategoryName);
            CreateOrUpdateDebugLog(uniqueID);
            UpdateLogCount(uniqueID);
        }
        
        private void OnNewConstantLogged(int uniqueID)
        {
            CreateConstantLogCategoryIfNotExists(VRDebug.LogCategoriesDict[uniqueID].CategoryName);
            CreateOrUpdateConstantLog(uniqueID);
        }

        private void CreateDebugLogCategoryIfNotExists(string categoryName)
        {
            if (!_debugLogCategories.ContainsKey(categoryName))
            {
                var newCategory = Instantiate(debugLogCategoryItemPrefab, debugLogsParent);
                newCategory.Init(categoryName);
                _debugLogCategories.Add(categoryName, newCategory);
            }
        }
        private void CreateConstantLogCategoryIfNotExists(string categoryName)
        {
            if (!_constantLogCategories.ContainsKey(categoryName))
            {
                var newCategory = Instantiate(constantCategoryItemPrefab, constantLogsParent);
                newCategory.Init(categoryName);
                _constantLogCategories.Add(categoryName, newCategory);
            }
        }

        private void CreateOrUpdateDebugLog(int uniqueID)
        {
            if (_debugLogs.TryGetValue(uniqueID, out List<DebugLogItem> logItems))
            {
                if (VRDebug.LogCategoriesDict[uniqueID].Collapsed)
                {
                    var log = VRDebug.DebugLogsDict[uniqueID];
                    log.LogCount++;
                    VRDebug.DebugLogsDict[uniqueID] = log;
                    logItems[0].Refresh(uniqueID);
                    return;
                }
            }
            else
            {
                logItems = new List<DebugLogItem>();
            }

            var category = _debugLogCategories[VRDebug.LogCategoriesDict[uniqueID].CategoryName];
            var newLogItem = Instantiate(debugLogItemPrefab, category.Layout);
            newLogItem.Init(uniqueID, GetLogIcon(VRDebug.DebugLogsDict[uniqueID].vrLogType));
            if (logItems.Count == 0)
                _debugLogs.Add(uniqueID, logItems);
            logItems.Add(newLogItem);
            newLogItem.gameObject.SetActive(_logVisibles[VRDebug.DebugLogsDict[uniqueID].vrLogType]);
        }
        
        private void CreateOrUpdateConstantLog(int uniqueID)
        {
            if (_constantLogs.TryGetValue(uniqueID, out ConstantLogItem logItem))
            {
                logItem.Refresh(uniqueID);
                return;
            }

            var category = _constantLogCategories[VRDebug.LogCategoriesDict[uniqueID].CategoryName];
            var newLogItem = Instantiate(constantLogItemPrefab, category.Layout);
            newLogItem.Init(VRDebug.ConstantLogsDict[uniqueID].ConstantName);
            newLogItem.Refresh(uniqueID);
            _constantLogs.Add(uniqueID, newLogItem);
        }

        private void UpdateLogCount(int uniqueID)
        {
            _logCounts[VRDebug.DebugLogsDict[uniqueID].vrLogType]++;

            infoControlButton.SetCount(_logCounts[VRLogType.Info]);
            warningControlButton.SetCount(_logCounts[VRLogType.Warning]);
            errorControlButton.SetCount(_logCounts[VRLogType.Error]);
        }

        private void OnPressedClearButton()
        {
            if (_debugLogCategories.Count == 0)
                return;

            foreach (var pair in _debugLogCategories)
            {
                Destroy(pair.Value.gameObject);
            }
            _debugLogs.Clear();
            _debugLogCategories.Clear();
            var logTypeList = Enum.GetValues(typeof(VRLogType));
            foreach (var logType in logTypeList)
            {
                _logVisibles[(VRLogType)logType] = true;
                _logCounts[(VRLogType)logType] = 0;
            }
            
            VRDebug.DebugLogsDict.Clear();
            VRDebug.LogCategoriesDict.Clear();
            
            infoControlButton.SetCount(0);
            warningControlButton.SetCount(0);
            errorControlButton.SetCount(0);
        }

        private void OnPressedControlButton(VRLogType type)
        {
            _logVisibles[type] = !_logVisibles[type];

            foreach (var pair in VRDebug.DebugLogsDict)
            {
                if (pair.Value.vrLogType == type)
                {
                    for (int i = 0; i < _debugLogs[pair.Key].Count; i++)
                    {
                        _debugLogs[pair.Key][i].gameObject.SetActive(_logVisibles[type]);
                    }
                }
            }

            ChangeCategoriesVisibility();
        }

        private void ChangeCategoriesVisibility()
        {
            foreach (var pair in _debugLogCategories)
            {
                pair.Value.gameObject.SetActive(pair.Value.GetActiveLogCount() > 0);
            }
        }
        
        private Sprite GetLogIcon(VRLogType vrLogType)
        {
            switch (vrLogType)
            {
                case VRLogType.Info:
                    return infoIcon;
                case VRLogType.Warning:
                    return warningIcon;
                case VRLogType.Error:
                    return errorIcon;
                default:
                    return infoIcon;
            }
        }
    }
}