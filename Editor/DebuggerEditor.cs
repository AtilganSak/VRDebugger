using HeatInteractive.VRDebugger;
using UnityEditor;
using UnityEngine;

namespace HeatInteractive
{
    public class DebuggerEditor
    {
        [MenuItem("GameObject/Heat Interactive/Setup VRDebugger")]
        private static void SetupVRDebugger()
        {
            var prefab = AssetDatabase.LoadAssetAtPath<VRDebuggerController>("Packages/com.heatinteractive.vrdebugger/Runtime/Prefabs/VRDebugger.prefab");
            var instance = PrefabUtility.InstantiatePrefab(prefab) as VRDebuggerController;
            instance.name =instance.name.Replace("(Clone)", ""); 
            Selection.activeGameObject = instance.gameObject;
        }
    }
}