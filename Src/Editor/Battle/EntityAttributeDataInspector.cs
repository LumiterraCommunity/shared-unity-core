/*
 * @Author: xiang huan
 * @Date: 2023-01-16 09:44:22
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Editor/Battle/EntityAttributeDataInspector.cs
 * 
 */
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SharedCore.Editor
{
    [CustomEditor(typeof(EntityAttributeData))]
    internal sealed class EntityAttributeDataInspector : UnityEditor.Editor
    {
        private readonly HashSet<string> _openedItems = new();
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("仅运行时可用", MessageType.Info);
                return;
            }

            EntityAttributeData entityAttributeData = (EntityAttributeData)target;

            foreach (KeyValuePair<eAttributeType, IntAttribute> item in entityAttributeData.AttributeMap)
            {
                DrawEntityAttribute(item);
            }

            Repaint();
        }
        private void DrawEntityAttribute(KeyValuePair<eAttributeType, IntAttribute> item)
        {
            DREntityAttribute dREntityAttribute = GFEntryCore.DataTable.GetDataTable<DREntityAttribute>().GetDataRow((int)item.Key);
            string name = "";
            if (dREntityAttribute != null)
            {
                name = dREntityAttribute.Name;
            }
            bool lastState = _openedItems.Contains(name);
            bool currentState = EditorGUILayout.Foldout(lastState, name);
            if (currentState != lastState)
            {
                if (currentState)
                {
                    _openedItems.Add(name);
                }
                else
                {
                    _openedItems.Remove(name);
                }
            }

            if (currentState)
            {

                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Name", name);
                    EditorGUILayout.LabelField("Value(当前值)", item.Value.Value.ToString());
                    EditorGUILayout.LabelField("BaseValue(基础值)", item.Value.BaseValue.ToString());
                    EditorGUILayout.LabelField("Add(增加值)", item.Value.Add.ToString());
                    EditorGUILayout.LabelField("PctAdd(增加百分比)", item.Value.PctAdd.ToString());
                    EditorGUILayout.LabelField("FinalAdd(最终增加值)", item.Value.FinalAdd.ToString());
                    EditorGUILayout.LabelField("FinalPctAdd(最终百分比增加)", item.Value.FinalPctAdd.ToString());

                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Separator();
            }

        }

    }
}
