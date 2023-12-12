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
        private sealed class StringArrayListProcessor : GenericDataProcessor<string>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "string[][]";

            public override string ExtensionParseKey => "ParseArrayList<string>";

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "string[][]"
                };
            }

            public override string Parse(string value)
            {
                return null;
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(value);
            }
        }
    }
}
