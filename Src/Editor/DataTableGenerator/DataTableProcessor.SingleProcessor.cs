//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;

namespace Custom.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class SingleProcessor : GenericDataProcessor<float>
        {
            public override bool IsSystem => true;
            public override string LanguageKeyword => "float";
            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "single",
                    "system.single"
                };
            }

            public override float Parse(string value)
            {
                return float.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}
