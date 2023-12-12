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
        private sealed class IdProcessor : DataProcessor
        {
            public override System.Type Type => typeof(int);

            public override bool IsId => true;
            public override bool IsComment => false;
            public override bool IsSystem => false;
            public override string LanguageKeyword => "int";

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "id"
                };
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = "0";
                }
                binaryWriter.Write7BitEncodedInt32(DataTableParseUtil.ParseInt(value));
            }
        }
    }
}
