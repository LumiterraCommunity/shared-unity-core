//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.IO;

namespace Custom.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class Int32Processor : GenericDataProcessor<int>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "int";

            public override string ExtensionParseKey => "ParseInt";
            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "int",
                    "int32",
                    "system.int32"
                };
            }

            public override int Parse(string value)
            {
                return DataTableParseUtil.ParseInt(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write7BitEncodedInt32(Parse(value));
            }
        }
    }
}
