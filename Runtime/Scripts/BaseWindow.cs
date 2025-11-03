using System;
using UnityEngine;

namespace HeatInteractive.VRDebugger
{
    public class BaseWindow : MonoBehaviour
    {
        [SerializeField] private int windowIndex;
        public int WindowIndex => windowIndex;

        [SerializeField] private Transform root;

        protected virtual void Awake()
        {
            
        }

        public virtual void Show()
        {
            root.gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            root.gameObject.SetActive(false);
        }
    }
}
