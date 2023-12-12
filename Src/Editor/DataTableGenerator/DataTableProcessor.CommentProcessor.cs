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
        private sealed class CommentProcessor : DataProcessor
        {
            public override System.Type Type => null;

            public override bool IsId => false;

            public override bool IsComment => true;
            public override bool IsSystem => false;

            public override string LanguageKeyword => null;
            public override string[] GetTypeStrings()
            {
                return new string[]
                {
                    string.Empty,
                    "#",
                    "comment",
                    "null"
                };
            }

            public override void WriteToStream(DataTableProcessor dataTableProcessor, BinaryWriter binaryWriter, string value)
            {
            }
        }
    }
}
