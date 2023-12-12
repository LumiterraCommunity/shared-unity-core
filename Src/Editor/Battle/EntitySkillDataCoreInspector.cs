/*
 * @Author: xiang huan
 * @Date: 2023-01-16 09:44:22
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Editor/Battle/EntitySkillDataCoreInspector.cs
 * 
 */
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SharedCore.Editor
{
    [CustomEditor(typeof(EntitySkillDataCore), true)]
    class EntitySkillDataCoreInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("仅运行时可用", MessageType.Info);
                return;
            }

            EntitySkillDataCore skillDataCore = (EntitySkillDataCore)target;
            EditorGUILayout.LabelField($"当前技能组列表");

            foreach (var item in skillDataCore.SkillGroupInfoDic)
            {
                _ = EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("技能组名:", item.Key.ToString());

                    for (int i = 0; i < item.Value.SkillIDArray.Length; i++)
                    {
                        EditorGUILayout.LabelField(item.Value.SkillIDArray[i].ToString());
                    }
                }
                EditorGUILayout.EndVertical();
            }



            Repaint();
        }

    }
}
