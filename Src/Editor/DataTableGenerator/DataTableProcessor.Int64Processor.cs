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
        private sealed class Int64Processor : GenericDataProcessor<long>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "long";
            public override string ExtensionParseKey => "ParseLong";

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "long",
                    "int64",
                    "system.int64"
                };
            }

            public override long Parse(string value)
            {
                return DataTableParseUtil.ParseLong(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write7BitEncodedInt64(Parse(value));
            }
        }
    }
}
