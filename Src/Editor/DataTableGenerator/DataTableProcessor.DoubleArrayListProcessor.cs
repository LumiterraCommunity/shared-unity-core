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
        private sealed class DoubleArrayListProcessor : GenericDataProcessor<double>
        {
            public override bool IsSystem => true;

            public override string LanguageKeyword => "double[][]";

            public override string ExtensionParseKey => "ParseArrayList<double>";

            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    "double[][]",
                    "float[][]",
                };
            }

            public override double Parse(string value)
            {
                return 0;
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
                binaryWriter.Write(value);
            }
        }
    }
}
