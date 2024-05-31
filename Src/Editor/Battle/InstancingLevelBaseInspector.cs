/*
 * @Author: xiang huan
 * @Date: 2023-01-16 09:44:22
 * @Description: 
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Editor/Battle/InstancingLevelBaseInspector.cs
 * 
 */
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SharedCore.Editor
{
    [CustomEditor(typeof(InstancingLevelBase), true)]
    internal class InstancingLevelBaseInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("仅运行时可用", MessageType.Info);
                return;
            }

            InstancingLevelBase instancingLevelBase = (InstancingLevelBase)target;
            EditorGUILayout.LabelField($"当前事件列表");
            for (int i = 0; i < instancingLevelBase.SceneTriggerEvents.Count; i++)
            {
                _ = EditorGUILayout.BeginVertical("box");
                SceneTriggerEvent sceneTriggerEvent = instancingLevelBase.SceneTriggerEvents[i];
                EditorGUILayout.LabelField("事件ID:", sceneTriggerEvent.Id.ToString());
                EditorGUILayout.LabelField("事件Cid", sceneTriggerEvent.DRSceneEvent.Id.ToString());
                EditorGUILayout.LabelField("事件状态", sceneTriggerEvent.StatusType.ToString());

                EditorGUILayout.LabelField($"事件条件列表");
                for (int j = 0; j < sceneTriggerEvent.Conditions.Count; j++)
                {
                    _ = EditorGUILayout.BeginVertical("box");
                    STConditionBase condition = sceneTriggerEvent.Conditions[j];
                    EditorGUILayout.LabelField("条件ID:", condition.DRSceneEventCondition.Id.ToString());
                    EditorGUILayout.LabelField("条件数据", condition.GetNetData().ToString());
                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.LabelField($"事件行为列表");
                for (int j = 0; j < sceneTriggerEvent.Actions.Count; j++)
                {
                    _ = EditorGUILayout.BeginVertical("box");
                    STActionBase action = sceneTriggerEvent.Actions[j];
                    EditorGUILayout.LabelField("行为ID:", action.DRSceneEventAction.Id.ToString());
                    EditorGUILayout.LabelField("行为说明:", action.DRSceneEventAction.Desc);
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }


            Repaint();
        }

    }
}
