/*
 * @Author: xiang huan
 * @Date: 2023-01-16 09:44:22
 * @Description: 
 * @FilePath: /lumiterra-unity/Assets/Plugins/SharedCore/Src/Editor/Battle/SkillCptInspector.cs
 * 
 */
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

namespace SharedCore.Editor
{
    [CustomEditor(typeof(SkillCpt))]
    internal sealed class SkillCptInspector : UnityEditor.Editor
    {
        private string _skillID;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("仅运行时可用", MessageType.Info);
                return;
            }

            SkillCpt skillCpt = (SkillCpt)target;
            _ = EditorGUILayout.BeginHorizontal("box");
            {
                _skillID = EditorGUILayout.TextField("技能ID", _skillID);

                if (GUILayout.Button("添加"))
                {
                    if (int.TryParse(_skillID, out int value))
                    {
                        _ = skillCpt.AddSkill(value);
                    }

                }
                if (GUILayout.Button("删除"))
                {
                    if (int.TryParse(_skillID, out int value))
                    {
                        skillCpt.RemoveSkill(value);
                    }
                }
                if (GUILayout.Button("释放"))
                {
                    if (int.TryParse(_skillID, out int value))
                    {
                        Vector3 dir = skillCpt.RefEntity.Forward;
                        SkillBase skillBase = skillCpt.GetSkill(value);
                        if (!skillCpt.CanUseSkill(value, dir))
                        {
                            return;
                        }
                        List<long> enemyList = new();
                        List<Vector3> targetPosList = new();

                        int targetNum = skillBase.DRSkill.IsRemote ? skillBase.DRSkill.SkillFlyerNum : 1;
                        if (skillCpt.TryGetComponent(out EntitySkillSearchTarget skillSearchTarget))
                        {
                            skillSearchTarget.SearchTarget(value, targetNum);
                            if (skillSearchTarget.TargetList.Count > 0)
                            {
                                dir = skillSearchTarget.TargetList[0].Position - skillCpt.RefEntity.Position;
                                dir.Normalize();
                                for (int i = 0; i < skillSearchTarget.TargetList.Count; i++)
                                {
                                    enemyList.Add(skillSearchTarget.TargetList[i].BaseData.Id);
                                    targetPosList.Add(skillSearchTarget.TargetList[i].Position);
                                }
                            }
                        }

                        InputSkillReleaseData inputData = new(value, dir, enemyList.ToArray(), targetPosList.ToArray());
                        skillCpt.RefEntity.EntityEvent.InputSkillRelease?.Invoke(inputData);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("当前技能数量", skillCpt.SkillMap.Count.ToString());

            foreach (KeyValuePair<int, SkillBase> item in skillCpt.SkillMap)
            {
                _ = EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField(item.Key.ToString(), item.Value.DRSkill.SkillName);
                }
                EditorGUILayout.EndVertical();
            }

            Repaint();
        }

    }
}
