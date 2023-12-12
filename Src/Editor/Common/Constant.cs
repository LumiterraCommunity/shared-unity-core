using System.IO;

namespace SharedCore.Editor
{
    public static class Constant
    {

        // 用户预设key
        public enum ePlayerPrefsKey
        {
            SVN_CONFING_FOLDER_PATH,
            PROTOS_FOLDER_PATH
        }

        // 目录分割符
        public static readonly char Spt = Path.DirectorySeparatorChar;
        // 系统的 /bin/sh 命令
        public static readonly string BinSh = Path.Combine("/bin/sh");
        // 项目路径 lumiterra-unity
        public static readonly string ProjectPath = System.Environment.CurrentDirectory;
        // 项目Assets资源路径 lumiterra-unity/Assets
        public static readonly string AssetsPath = Path.Combine(ProjectPath, "Assets");
        // SharedCore路径 lumiterra-unity/Assets/Plugins/SharedCore
        public static readonly string SharedCorePath = Path.Combine(AssetsPath, "Plugins/SharedCore");
        // 项目临时文件夹 lumiterra-unity/Temp
        public static readonly string TempPath = Path.Combine(ProjectPath, "Temp");
        // 编辑器插件 lumiterra-unity/Assets/Plugins/SharedCore/Src/Editor 
        public static readonly string EditorPath = Path.Combine(SharedCorePath, "Src/Editor");
        // 通用的用于执行命令的sh文件 lumiterra-unity/Assets/Plugins/SharedCore/Src/Editor/Common/CommonBash.sh
        public static readonly string CommonHandleSh = Path.Combine(EditorPath, "Common/CommonBash.sh");
    }

}
