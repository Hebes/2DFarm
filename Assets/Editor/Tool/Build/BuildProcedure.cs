using System.IO;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    打包成程序

-----------------------*/

namespace ACFrameworkCore
{
    /// <summary>
    /// 平台类型
    /// </summary>
    public enum PlatformType
    {
        None,
        Android,
        IOS,
        PC,
        MacOS,
    }

    /// <summary>
    /// 打包类型
    /// </summary>
    public enum BuildType
    {
        /// <summary> 开发版本 </summary>
        Development,
        /// <summary> 公开版本 </summary>
        Release,
    }

    public class BuildProcedure : EditorWindow
    {
        private const string relativeDirPrefix = "../Release";
        public static string BuildFolder = "../Release/{0}/StreamingAssets/";

        private PlatformType activePlatform;//平台类型
        private PlatformType platformType;//平台类型
        private bool clearFolder;//清理文件夹
        private bool isBuildExe;//是否打包成EXE
        private bool isContainAB;//是否包含AB包
        private BuildType buildType;//打包类型
        private BuildOptions buildOptions;//打包选项
        private BuildAssetBundleOptions buildAssetBundleOptions = BuildAssetBundleOptions.None;//是否打AB包

        private void OnEnable()
        {
#if UNITY_ANDROID
			activePlatform = PlatformType.Android;
#elif UNITY_IOS
			activePlatform = PlatformType.IOS;
#elif UNITY_STANDALONE_WIN
            activePlatform = PlatformType.PC;
#elif UNITY_STANDALONE_OSX
			activePlatform = PlatformType.MacOS;
#else
			activePlatform = PlatformType.None;
#endif
            platformType = activePlatform;
        }

        //来自ET框架
        [MenuItem("Tool/打包")]//#E
        public static void BuildPackage()
        {
            GetWindow(typeof(BuildEditor));
        }

        private void OnGUI()
        {
            //1.设置选项
            EditorGUILayout.LabelField("打包平台:");
            this.platformType = (PlatformType)EditorGUILayout.EnumPopup(platformType);
            this.clearFolder = EditorGUILayout.Toggle("清理资源文件夹: ", clearFolder);
            this.isBuildExe = EditorGUILayout.Toggle("是否打包EXE: ", this.isBuildExe);
            //this.isContainAB = EditorGUILayout.Toggle("是否同将资源打进EXE: ", this.isContainAB);
            this.buildType = (BuildType)EditorGUILayout.EnumPopup("BuildType: ", this.buildType);
            EditorGUILayout.LabelField("BuildAssetBundleOptions(可多选):");
            this.buildAssetBundleOptions = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField(this.buildAssetBundleOptions);
            switch (buildType)
            {
                case BuildType.Development: this.buildOptions = BuildOptions.Development | BuildOptions.ConnectWithProfiler; break;
                case BuildType.Release: this.buildOptions = BuildOptions.None; break;
            }
            GUILayout.Space(5);
            //2.开始打包
            if (GUILayout.Button("开始打包"))
            {
                if (this.platformType == PlatformType.None)
                {
                    ShowNotification(new GUIContent("请选择打包平台!"));
                    return;
                }
                if (platformType != activePlatform)
                {
                    switch (EditorUtility.DisplayDialogComplex("警告!", $"当前目标平台为{activePlatform}, 如果切换到{platformType}, 可能需要较长加载时间", "切换", "取消", "不切换"))
                    {
                        case 0: activePlatform = platformType; break;
                        case 1: return;
                        case 2: platformType = activePlatform; break;
                    }
                }
                BuildProcedure.Build(this.platformType, this.buildAssetBundleOptions, this.buildOptions, this.isBuildExe, this.isContainAB, this.clearFolder);
            }

            GUILayout.Space(5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="buildAssetBundleOptions"></param>
        /// <param name="buildOptions"></param>
        /// <param name="isBuildExe">是否打包Exe</param>
        /// <param name="isContainAB"></param>
        /// <param name="clearFolder"></param>
        public static void Build(PlatformType type, BuildAssetBundleOptions buildAssetBundleOptions, BuildOptions buildOptions, bool isBuildExe, bool isContainAB, bool clearFolder)
        {
            BuildTarget buildTarget = BuildTarget.StandaloneWindows;
            string programName = "Test打包";
            string exeName = programName;
            switch (type)
            {
                case PlatformType.PC:
                    buildTarget = BuildTarget.StandaloneWindows64;
                    exeName += ".exe";
                    break;
                case PlatformType.Android:
                    buildTarget = BuildTarget.Android;
                    exeName += ".apk";
                    break;
                case PlatformType.IOS:
                    buildTarget = BuildTarget.iOS;
                    break;
                case PlatformType.MacOS:
                    buildTarget = BuildTarget.StandaloneOSX;
                    break;
            }

            //打包输出地址
            string fold = string.Format(BuildFolder, type);
            if (clearFolder && Directory.Exists(fold))
                Directory.Delete(fold, true);
            Directory.CreateDirectory(fold);


            UnityEngine.Debug.Log("开始资源打包");
            //BuildPipeline.BuildAssetBundles(fold, buildAssetBundleOptions, buildTarget);

            //UnityEngine.Debug.Log("完成资源打包");

            //if (isContainAB)
            //{
            //    FileHelper.CleanDirectory("Assets/StreamingAssets/");
            //    FileHelper.CopyDirectory(fold, "Assets/StreamingAssets/");
            //}

            if (isBuildExe)
            {
                AssetDatabase.Refresh();
                string[] levels = {
                    "Assets/AssetsPackage/Scenes/Init.unity",
                };
                UnityEngine.Debug.Log("开始EXE打包");
                BuildPipeline.BuildPlayer(levels, $"{relativeDirPrefix}/{exeName}", buildTarget, buildOptions);
                UnityEngine.Debug.Log("完成exe打包");
            }
            else
            {
                //if (isContainAB && type == PlatformType.PC)
                //{
                //    string targetPath = Path.Combine(relativeDirPrefix, $"{programName}_Data/StreamingAssets/");
                //    FileHelper.CleanDirectory(targetPath);
                //    Debug.Log($"src dir: {fold}    target: {targetPath}");
                //    FileHelper.CopyDirectory(fold, targetPath);
                //}
            }
        }
    }
}
