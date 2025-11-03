using System;
using System.Collections.Generic;
using heatinteractive.VRDebugger;
using UnityEngine;
using UnityEngine.UI;

namespace HeatInteractive.VRDebugger
{
    public class Window_Console : BaseWindow
    {
        [Header("Category")]
        [SerializeField] private CategoryItem categoryItemPrefab;

        [Header("Debug Log")]
        [SerializeField] private DebugLogItem debugLogItemPrefab;

        [Header("Buttons")]
        [SerializeField] private Button clearButton;

        [SerializeField] private ControlButton infoControlButton;
        [SerializeField] private ControlButton warningControlButton;
        [SerializeField] private ControlButton errorControlButton;

        [Header("Common")]
        [SerializeField] private Transform logsParent;

        [SerializeField] private Sprite infoIcon;
        [SerializeField] private Sprite warningIcon;
        [SerializeField] private Sprite errorIcon;

        private Dictionary<int, List<DebugLogItem>> _debugLogs = new();
        private Dictionary<string, CategoryItem> _categories = new();
        private Dictionary<VRLogType, int> _logCounts = new();
        private Dictionary<VRLogType, bool> _logVisibles = new();

        protected override void Awake()
        {
            base.Awake();

            VRDebug.SubscribeDebugLogEvent(OnNewDebugLogged);

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
        }

        private void OnNewDebugLogged(int uniqueID)
        {
            CreateCategoryIfNotExists(VRDebug.DebugLogCategoriesDict[uniqueID].CategoryName);
            CreateOrUpdateDebugLog(uniqueID);
            UpdateLogCount(uniqueID);
        }

        private void CreateCategoryIfNotExists(string categoryName)
        {
            if (!_categories.ContainsKey(categoryName))
            {
                var newCategory = Instantiate(categoryItemPrefab, logsParent);
                newCategory.Init(categoryName);
                _categories.Add(categoryName, newCategory);
            }
        }

        private void CreateOrUpdateDebugLog(int uniqueID)
        {
            if (_debugLogs.TryGetValue(uniqueID, out List<DebugLogItem> logItems))
            {
                if (VRDebug.DebugLogCategoriesDict[uniqueID].Collapsed)
                {
                    var log = VRDebug.DebugLogsDict[uniqueID];
                    log.LogCount++;
                    VRDebug.DebugLogsDict[uniqueID] = log;
                    logItems[0].Refresh();
                    return;
                }
            }
            else
            {
                logItems = new List<DebugLogItem>();
            }

            var category = _categories[VRDebug.DebugLogCategoriesDict[uniqueID].CategoryName];
            var newLogItem = Instantiate(debugLogItemPrefab, category.Layout);
            newLogItem.Init(uniqueID, GetLogIcon(VRDebug.DebugLogsDict[uniqueID].vrLogType));
            if (logItems.Count == 0)
                _debugLogs.Add(uniqueID, logItems);
            logItems.Add(newLogItem);
            newLogItem.gameObject.SetActive(_logVisibles[VRDebug.DebugLogsDict[uniqueID].vrLogType]);
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
            if (_categories.Count == 0)
                return;

            foreach (var pair in _categories)
            {
                Destroy(pair.Value.gameObject);
            }
            _debugLogs.Clear();
            _categories.Clear();
            var logTypeList = Enum.GetValues(typeof(VRLogType));
            foreach (var logType in logTypeList)
            {
                _logVisibles[(VRLogType)logType] = true;
                _logCounts[(VRLogType)logType] = 0;
            }
            
            VRDebug.DebugLogsDict.Clear();
            VRDebug.DebugLogCategoriesDict.Clear();
            
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
            foreach (var pair in _categories)
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