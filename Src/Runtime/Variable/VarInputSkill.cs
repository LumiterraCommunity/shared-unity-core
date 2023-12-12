/*
 * @Author: xiang huan
 * @Date: 2023-02-09 16:04:59
 * @Description: 输入技能数据
 * @FilePath: /lumiterra-scene-server/Assets/Plugins/SharedCore/Src/Runtime/Variable/VarInputSkill.cs
 * 
 */


using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// UnityEngine.Object 变量类。
    /// </summary>
    public sealed class VarInputSkill : Variable<InputSkillReleaseData>
    {
        /// <summary>
        /// 初始化 UnityEngine.Object 变量类的新实例。
        /// </summary>
        public VarInputSkill()
        {
        }

        /// <summary>
        /// 从 UnityEngine.Object 到 UnityEngine.Object 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarInputSkill(InputSkillReleaseData value)
        {
            VarInputSkill varValue = ReferencePool.Acquire<VarInputSkill>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 UnityEngine.Object 变量类到 UnityEngine.Object 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator InputSkillReleaseData(VarInputSkill value)
        {
            return value.Value;
        }
    }
}
