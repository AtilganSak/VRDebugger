using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HeatInteractive.VRDebugger
{
    public class CategoryItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text headerText;
        [SerializeField] private Transform layout;
        [SerializeField] private RectTransform rectTransform;

        public Transform Layout => layout;

        public void Init(string categoryName)
        {
            headerText.text = categoryName;
        }

        public void RebuildLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

        public int GetActiveLogCount()
        {
            int logCount = 0;
            int childCount = layout.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (layout.GetChild(i).gameObject.activeSelf)
                    logCount++;
            }
            return logCount;
        }
    }
}