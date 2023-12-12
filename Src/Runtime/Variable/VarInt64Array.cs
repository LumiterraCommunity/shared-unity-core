//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// System.Int64 数组变量类。
    /// </summary>
    public sealed class VarInt64Array : Variable<long[]>
    {
        /// <summary>
        /// 初始化 System.Int64 数组变量类的新实例。
        /// </summary>
        public VarInt64Array()
        {
        }

        /// <summary>
        /// 从 System.Int64 数组到 System.Int64 数组变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarInt64Array(long[] value)
        {
            VarInt64Array varValue = ReferencePool.Acquire<VarInt64Array>();
            varValue.Value = value;
            return varValue;
        }

        /// <summary>
        /// 从 System.Int64 数组变量类到 System.Int64 数组的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator long[](VarInt64Array value)
        {
            return value.Value;
        }
    }
}
