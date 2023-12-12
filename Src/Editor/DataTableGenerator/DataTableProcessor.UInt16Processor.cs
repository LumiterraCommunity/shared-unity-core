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
        private sealed class UInt16Processor : GenericDataProcessor<ushort>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "ushort";

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "ushort",
                    "uint16",
                    "system.uint16"
                };
            }

            public override ushort Parse(string value)
            {
                return ushort.Parse(value);
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(Parse(value));
            }
        }
    }
}
