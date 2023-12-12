//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Custom.Editor.DataTableTools
{
    public sealed partial class DataTableProcessor
    {
        private sealed class BoolArrayListProcessor : GenericDataProcessor<bool>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "bool[][]";

            public override string ExtensionParseKey => "ParseArrayList<bool>";

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "bool[][]",
                };
            }

            public override bool Parse(string value)
            {
                return true;
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(value);
            }
        }
    }
}
