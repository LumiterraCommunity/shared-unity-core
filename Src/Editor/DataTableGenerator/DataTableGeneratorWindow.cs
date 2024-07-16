using UnityEditor;
using UnityEngine;
using SharedCore.Editor;

namespace Custom.Editor.DataTableTools
{
    public class DataTableGeneratorWindow : EditorWindow
    {
        private string _configPath;
        [MenuItem("devtool/csv")]
        public static void Init()
        {
            GetWindow<DataTableGeneratorWindow>().Show();
        }

        private void OnGUI()
        {
            //水平布局
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("*.svn表配置路径:", GUILayout.Width(80f));
                _configPath = GUILayout.TextField(_configPath);

                string preFolderPath = PlayerPrefs.GetString(Constant.ePlayerPrefsKey.SVN_CONFING_FOLDER_PATH.ToString());
                if (!string.IsNullOrEmpty(preFolderPath))
                {
                    _configPath = preFolderPath;
                }
                DataTableGeneratorUtil.SetPath(_configPath, "/csv");
                if (GUILayout.Button("浏览", GUILayout.Width(50f)))
                {
                    _configPath = EditorUtility.OpenFolderPanel("select path", _configPath, "");
                    if (!string.IsNullOrEmpty(_configPath))
                    {
                        PlayerPrefs.SetString(Constant.ePlayerPrefsKey.SVN_CONFING_FOLDER_PATH.ToString(), _configPath);
                        PlayerPrefs.Save();
                    }
                    DataTableGeneratorUtil.SetPath(_configPath, "/csv");
                }

            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("生成配置"))
            {
                DataTableGeneratorUtil.SetPath(_configPath , "/csv");
                DataTableGeneratorUtil.UpdateCsv();
                DataTableGeneratorUtil.GenerateDataTables();
                DataTableGeneratorUtil.GenerateAttributeTypeFile();
                SharedCoresVersionTool.UpdateCsvVersion();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("生成配置脚本"))
            {
                DataTableGeneratorUtil.SetPath(_configPath , "/csv");
                DataTableGeneratorUtil.GenerateDataTableCodes();
                SharedCoresVersionTool.UpdateCsvVersion();
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("一键生成") )
            {
                DataTableGeneratorUtil.SetPath(_configPath , "/csv");
                RefreshAll();
            }

            if (GUILayout.Button("一键生成-Sonic"))
            {
                DataTableGeneratorUtil.SetPath(_configPath , "/csv-Sonic");
                RefreshAll();
            }

            if (GUILayout.Button("一键生成-Ronin"))
            {
                DataTableGeneratorUtil.SetPath(_configPath , "/csv-Ronin");
                RefreshAll();
            }

        }

        private void RefreshAll()
        {
            DataTableGeneratorUtil.UpdateCsv();
            DataTableGeneratorUtil.GenerateDataTables();
            DataTableGeneratorUtil.GenerateAttributeTypeFile();
            DataTableGeneratorUtil.GenerateDataTableCodes();
            SharedCoresVersionTool.UpdateCsvVersion();
            AssetDatabase.Refresh();
        }
    }
}