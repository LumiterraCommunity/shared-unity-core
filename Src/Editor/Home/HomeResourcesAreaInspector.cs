/*
 * @Author: xiang huan
 * @Date: 2023-03-10 10:24:09
 * @Description: 资源区域
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Editor/Home/HomeResourcesAreaInspector.cs
 * 
 */

using UnityEditor;
using UnityEngine;

namespace SharedCore.Editor
{
    [CustomEditor(typeof(HomeResourcesArea))]
    [CanEditMultipleObjects]
    internal sealed class HomeResourcesAreaInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // if (GUILayout.Button("生成唯一ID"))
            // {

            //     for (int i = 0; i < targets.Length; i++)
            //     {
            //         HomeResourcesArea homeResourcesArea = targets[i] as HomeResourcesArea;
            //         if (homeResourcesArea == null) continue;
            //         GlobalObjectId globalObjectId = GlobalObjectId.GetGlobalObjectIdSlow(targets[i]);
            //         homeResourcesArea.Id = globalObjectId.targetPrefabId.GetHashCode();
            //         EditorUtility.SetDirty(homeResourcesArea);
            //     }
            // }
        }
    }
}
