using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;

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

    public class BuildEditor : EditorWindow
    {
        [MenuItem("Tool/克隆初始化项目Alt+Q")]//#E
        public static void InitProject()
        {
            //请先HybridCLR/Install查看是否安装
            //点击生成HybridCLR/Generate/ALl(报错如果显示先打包,请先打包,其他报错请联系作者)
            //打包:HybridCLR/Build/Win64
        }

        [MenuItem("Tool/快速打包热更DLl _F5")]
        public static void BuildDLL()
        {
            CompileDllCommand.CompileDllActiveBuildTarget();
            BuildAssetsCommand.BuildAndCopyABAOTHotUpdateDlls();
        }

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
                BuildHelper.Build(this.platformType, this.buildAssetBundleOptions, this.buildOptions, this.isBuildExe, this.isContainAB, this.clearFolder);
            }

            GUILayout.Space(5);
        }
    }
}
