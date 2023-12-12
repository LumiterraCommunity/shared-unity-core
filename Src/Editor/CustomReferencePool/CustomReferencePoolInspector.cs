/*
 * @Author: xiang huan
 * @Date: 2023-01-16 09:44:22
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Editor/CustomReferencePool/CustomReferencePoolInspector.cs
 * 
 */
using System.Collections.Generic;
using GameFramework;
using UnityEditor;

namespace SharedCore.Editor
{
    [CustomEditor(typeof(CustomReferencePoolComponent))]
    internal sealed class CustomReferencePoolInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.LabelField("Class Name", "Unused\tUsing\tAcquire\tRelease\tAdd\tRemove");
            foreach (KeyValuePair<System.Type, CustomReferenceCollection> item in CustomReferencePool.ReferenceCollections)
            {
                CustomReferenceCollection collection = item.Value;
                EditorGUILayout.LabelField(collection.ReferenceType.FullName, Utility.Text.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", collection.UnusedReferenceCount, collection.UsingReferenceCount, collection.AcquireReferenceCount, collection.ReleaseReferenceCount, collection.AddReferenceCount, collection.RemoveReferenceCount));
            }
            Repaint();
        }

    }
}
