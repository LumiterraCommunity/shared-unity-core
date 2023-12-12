//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace Custom.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        public abstract class GenericDataProcessor<T> : DataProcessor
        {
            public override System.Type Type => typeof(T);


            public override bool IsId => false;
            public override bool IsComment => false;
            public abstract T Parse(string value);
        }
    }
}
