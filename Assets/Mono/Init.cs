using Farm2D;
using HybridCLR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using YooAsset;

/// <summary> YooAsset流程状态 </summary>
public enum EProcess
{
    /// <summary> 流程错误跳出 </summary>
    FsmErrorPrepare,
    /// <summary> 流程准备工作 </summary>
    FsmPatchPrepare,
    /// <summary> 检查版本XML配置文件 </summary>
    FsmVersionXMLPrepare,
    /// <summary> 初始化资源包 </summary>
    FsmInitialize,
    /// <summary> 更新资源版本号 </summary>
    FsmUpdateVersion,
    /// <summary> 更新资源清单 </summary>
    FsmUpdateManifest,
    /// <summary> 创建文件下载器 </summary>
    FsmCreateDownloader,
    /// <summary> 下载更新文件 </summary>
    FsmDownloadFiles,
    /// <summary> 下载完毕 </summary>
    FsmDownloadOver,
    /// <summary> 清理未使用的缓存文件 </summary>
    FsmClearCache,
    /// <summary> 流程更新完毕 </summary>
    FsmPatchDone,
    /// <summary> HybridCLR热更代码 </summary>
    FsmLoadHotDll,
}

public class Init : MonoBehaviour
{
#if UNITY_ANDROID
    private readonly string packageName = "Android";                                            //包名
#elif UNITY_STANDALONE_WIN
    private readonly string packageName = "PC";                                                 //包名
#endif

    //Yooasset相关
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;                                   //资源系统运行模式
    private EProcess CurrentProcess;                                                            //当前所在的流程
    private ResourceDownloaderOperation downloader;                                             //下载器
    public UpdatePackageVersionOperation operation;                                             //更新包192.168.2.4
    private static Assembly _hotUpdateAss;
    private string packageVersion;                                              // 更新成功后自动保存版本号，作为下次初始化的版本。也可以通过operation.SavePackageVersion()方法保存。
    //private string XMLVersionUrl = "http://127.0.0.1:8000/ACPackageVersion.xml";                //资源版本下载
    //private string SaveXMLVersion = $"{Application.streamingAssetsPath}/ACPackageVersion.xml";  //版本路径读取
    public static List<string> AOTMetaAssemblyNames = new List<string>()
    {
        "mscorlib.dll",
        "System.dll",
        "System.Core.dll",
        "UniTask.dll",
    };//补充元数据dll的列表
    private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();  //资源数据

    public GameObject goTemp { get; private set; }

    //UILoading界面
    private Text LoadingText;

    //开始加载
    private void Awake()
    {
        Debug.Log($"当前下载包名是：{packageName}");
        Debug.Log($"资源系统运行模式：{PlayMode}");
        Application.targetFrameRate = 60;//限定帧数
        Application.runInBackground = true;//是否后台开启
        FsmProcessChange(EProcess.FsmPatchPrepare);
    }
    private void FsmProcessChange(EProcess HotUpdateProcess)
    {
        switch (HotUpdateProcess)
        {
            case EProcess.FsmPatchPrepare: StartCoroutine(FsmPatchPrepare()); break;
            case EProcess.FsmVersionXMLPrepare: StartCoroutine(FsmVersionXMLPrepare()); break;
            case EProcess.FsmInitialize: StartCoroutine(FsmInitialize()); break;
            case EProcess.FsmUpdateVersion: StartCoroutine(FsmUpdateVersion()); break;
            case EProcess.FsmUpdateManifest: StartCoroutine(FsmUpdateManifest()); break;
            case EProcess.FsmCreateDownloader: StartCoroutine(FsmCreateDownloader()); break;
            case EProcess.FsmDownloadFiles: StartCoroutine(FsmDownloadFiles()); break;
            case EProcess.FsmDownloadOver: StartCoroutine(FsmDownloadOver()); break;
            case EProcess.FsmClearCache: StartCoroutine(FsmClearCache()); break;
            case EProcess.FsmPatchDone: StartCoroutine(FsmPatchDone()); break;
            case EProcess.FsmLoadHotDll: StartCoroutine(FsmLoadHotDll()); break;
            case EProcess.FsmErrorPrepare: StartCoroutine(FsmErrorPrepare()); break;
        }
        this.CurrentProcess = HotUpdateProcess;
    }//状态机流程切换



    /// <summary>
    /// 流程错误跳出
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmErrorPrepare()
    {
        Debug.Log($"结束中断,中断点:{CurrentProcess}");
        Debug.Log($"流程错误跳出");
        yield break;
    }

    /// <summary>
    /// 流程准备工作
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmPatchPrepare()
    {
        Debug.Log("流程准备工作");
        //加载日志
        GameObject debugGo = new GameObject("Debugger");
        debugGo.AddComponent<Debugger>();
        GameObject.DontDestroyOnLoad(debugGo);

        //TODO 暂时试用Resources
        GameObject go = Resources.Load<GameObject>("ResUILoading");
        goTemp = GameObject.Instantiate(go);
        LoadingText = goTemp.transform.Find("Canvas/UILoading/Image/Image (1)/T_Text").GetComponent<Text>();

        yield return null;
        LoadingText.text = "流程准备工作";

        // 初始化资源系统
        YooAssets.Initialize();
        YooAssets.SetOperationSystemMaxTimeSlice(30);//设置异步系统参数，每帧执行消耗的最大时间切片
        //TODO 加载更新面板
        FsmProcessChange(EProcess.FsmVersionXMLPrepare);
    }

    /// <summary>
    /// 检查版本XML配置文件
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmVersionXMLPrepare()
    {
        if (PlayMode == EPlayMode.HostPlayMode)
        {
            LoadingText.text = "检查版本配置文件";
            //yield return LoadAsset(XMLVersionUrl, SaveXMLVersion);
            //TODO 需要添加判断如果没下载成功的话
            yield return null;
        }
        //进入初始资源流程
        FsmProcessChange(EProcess.FsmInitialize);
    }

    /// <summary>
    /// 初始化资源包
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmInitialize()
    {
        Debug.Log("初始化资源包");
        yield return null;
        // 创建默认的资源包
        LoadingText.text = "初始化资源包";
        var package = YooAssets.TryGetPackage(packageName);
        if (package == null)
        {
            // 创建默认的资源包
            package = YooAssets.CreatePackage(packageName);
        }
        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);

        //进入模式
        InitializationOperation initializationOperation = null;
        switch (PlayMode)
        {
            case EPlayMode.EditorSimulateMode://编辑器下的模拟模式,在编辑器下，不需要构建资源包，来模拟运行游戏。
                var initParametersEditorSimulateMode = new EditorSimulateModeParameters();
                initParametersEditorSimulateMode.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
                initializationOperation = package.InitializeAsync(initParametersEditorSimulateMode);
                break;
            case EPlayMode.OfflinePlayMode://单机模式,离线运行模式,对于不需要热更新资源的游戏，可以使用单机运行模式。
                var initParametersOfflinePlayMode = new OfflinePlayModeParameters();
                initParametersOfflinePlayMode.DecryptionServices = new GameDecryptionServices();
                initializationOperation = package.InitializeAsync(initParametersOfflinePlayMode);
                break;
            case EPlayMode.HostPlayMode://联机运行模式,对于需要热更新资源的游戏，可以使用联机运行模式，该模式下初始化参数会很多。
                string defaultHostServer = GetHostServerURL();
                string fallbackHostServer = GetHostServerURL();
                var createParameters = new HostPlayModeParameters();
                createParameters.DecryptionServices = new GameDecryptionServices();
                createParameters.QueryServices = new GameQueryServices();
                createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                initializationOperation = package.InitializeAsync(createParameters);
                break;
        }

        yield return initializationOperation;
        if (initializationOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogError($"资源包初始化失败：{initializationOperation.Error}");
            FsmProcessChange(EProcess.FsmErrorPrepare);
            yield break;
        }
        Debug.Log("资源包初始化成功！");
        FsmProcessChange(EProcess.FsmUpdateVersion);
    }

    /// <summary>
    /// 更新资源版本号
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmUpdateVersion()
    {
        Debug.Log("更新资源版本号");
        yield return null;
        LoadingText.text = "更新资源版本号";
        var package = YooAssets.GetPackage(packageName);//获取包
        operation = package.UpdatePackageVersionAsync();
        yield return operation;
        if (operation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning($"更新资源版本号失败: {operation.Error}");
            //FsmProcessChange(EHotUpdateProcess.FsmErrorPrepare);
            Debug.Log($"将会使用旧版版本");
            FsmProcessChange(EProcess.FsmPatchDone);
            yield break;
        }
        packageVersion = operation.PackageVersion;
        Debug.Log($"更新补丁清单成功,远端或本地最新版本为: {operation.PackageVersion}");
        FsmProcessChange(EProcess.FsmUpdateManifest);
    }

    /// <summary>
    /// 更新资源清单
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmUpdateManifest()
    {
        yield return null;
        LoadingText.text = "更新资源清单";
        var package = YooAssets.GetPackage(packageName);//获取包
        // 更新成功后自动保存版本号，作为下次初始化的版本。
        // 也可以通过operation.SavePackageVersion()方法保存。
        bool savePackageVersion = true;

        Debug.Log("当前的版本号是:" + packageVersion);
        var operationResource = package.UpdatePackageManifestAsync(packageVersion, savePackageVersion);//operation.PackageVersion //
        yield return operationResource;

        if (operationResource.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning($"更新资源清单失败: {operationResource.Error}");
            FsmProcessChange(EProcess.FsmErrorPrepare);
            yield break;
        }
        Debug.Log($"更新资源清单成功: {operationResource.Status}!");
        FsmProcessChange(EProcess.FsmCreateDownloader);
    }

    /// <summary>
    /// 创建文件下载器
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmCreateDownloader()
    {
        yield return null;
        LoadingText.text = "创建文件下载器";
        var package = YooAssets.GetPackage(packageName);//获取包
        yield return null;

        int downloadingMaxNum = 10;//下载最大值
        int failedTryAgain = 3;//重试失败次数
        int timeout = 60;//超时时间
        downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain, timeout);

        //没有需要下载的资源
        if (downloader.TotalDownloadCount == 0)
        {
            Debug.Log("没有找到需要下载的文件! Not found any download files !");
            FsmProcessChange(EProcess.FsmDownloadOver);
            yield break;
        }
        Debug.Log($"找到需要下载{downloader.TotalDownloadCount}的文件! Found total {downloader.TotalDownloadCount} files that need download ！");
        LoadingText.text = $"找到需要下载{downloader.TotalDownloadCount}的文件!";
        // 发现新更新文件后，挂起流程系统
        // 注意：开发者需要在下载前检测磁盘空间不足
        //TODO 需要下载的文件总数和总大小
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;
        //TODO 开发者需要在下载前检测磁盘空间不足
        FsmProcessChange(EProcess.FsmDownloadFiles);
    }

    /// <summary>
    /// 下载更新文件
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmDownloadFiles()
    {
        yield return null;
        LoadingText.text = "下载更新文件";
        //注册回调方法
        downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;
        downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
        downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
        downloader.OnDownloadOverCallback = OnDownloadOverFunction;

        //开启下载
        downloader.BeginDownload();
        yield return downloader;

        //检测下载结果
        if (downloader.Status != EOperationStatus.Succeed)
        {
            Debug.LogError($"更新失败！{downloader.Error}");
            FsmProcessChange(EProcess.FsmErrorPrepare);
            yield break;
        }
        Debug.Log("更新完成!");
        FsmProcessChange(EProcess.FsmPatchDone);
    }

    /// <summary>
    /// 下载完毕
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmDownloadOver()
    {
        yield return null;
        FsmProcessChange(EProcess.FsmClearCache);
    }

    /// <summary>
    /// 清理未使用的缓存文件
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmClearCache()
    {
        yield return null;
        var package = YooAssets.GetPackage(packageName);
        var operation = package.ClearUnusedCacheFilesAsync();
        operation.Completed += OnClearCacheFunction;
        FsmProcessChange(EProcess.FsmPatchDone);
    }

    /// <summary>
    /// 流程更新完毕
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmPatchDone()
    {
        yield return null;
        FsmProcessChange(EProcess.FsmLoadHotDll);
    }

    /// <summary>
    /// HybridCLR热更代码
    /// </summary>
    /// <returns></returns>
    IEnumerator FsmLoadHotDll()
    {
        yield return null;
        LoadingText.text = "HybridCLR热更代码进入";

        #region 编辑器加载模式,用于打断点
        //进入的是编辑器加载热更模式,用于打断点
        if (PlayMode == EPlayMode.EditorSimulateMode)
        {
            //补充元数据HashSet
            string dllPath = $"{Application.dataPath}/AssetsPackage/HotDll";//热更新的DLL
            string[] filesPath = Directory.GetFiles(dllPath, "*.bytes", System.IO.SearchOption.AllDirectories);
            foreach (string filePath in filesPath)
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] buffur = new byte[fs.Length];
                fs.Read(buffur, 0, buffur.Length);
                fs.Close();
                HomologousImageMode mode = HomologousImageMode.SuperSet;
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(buffur, mode);
                Debug.Log($"LoadMetadataForAOTAssembly:{fs.Name}. mode:{mode} ret:{err}");
            }
            //进入游戏
#if !UNITY_EDITOR
         //_hotUpdateAss = Assembly.Load(ReadBytesFromStreamingAssets("HotUpdate.dll.bytes"));
#else
            _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
            _hotUpdateAss.GetType("InitGame").GetMethod("Init").Invoke(null, null);
            GameObject.Destroy(goTemp.gameObject);
            yield break;
        }
        #endregion

        LoadMetadataForAOTAssemblies();
        if (PlayMode == EPlayMode.OfflinePlayMode)
        {
            Debug.Log($"当前进入的是******************{PlayMode}******************");
            var package1 = YooAssets.GetPackage(packageName);
            //补充元数据
            foreach (var asset in AOTMetaAssemblyNames)
            {
                RawFileOperationHandle handle = package1.LoadRawFileAsync(asset);
                yield return handle;
                byte[] fileData = handle.GetRawFileData();
                s_assetDatas[asset] = fileData;
                Debug.Log($"dll名称是:{asset}  size大小为:{fileData.Length}");
            }
            //_hotUpdateAss = Assembly.Load(ReadBytesFromStreamingAssets("HotUpdate.dll.bytes"));
            _hotUpdateAss = Assembly.Load(File.ReadAllBytes($"{Application.dataPath}/AssetsPackage/HotDll/HotUpdate.dll.bytes"));
            _hotUpdateAss.GetType("InitGame").GetMethod("Init").Invoke(null, null);
            yield break;
        }

        #region 其他加载模式
        Debug.Log("进入了其他加载模式");
        //进入的其他模式
        var package = YooAssets.GetPackage(packageName);
#if !UNITY_EDITOR
        //System.Reflection.Assembly.Load(GetAssetData("HotUpdate.dll"));
#endif
        //补充元数据
        foreach (var asset in AOTMetaAssemblyNames)
        {
            RawFileOperationHandle handle = package.LoadRawFileAsync(asset);
            yield return handle;
            byte[] fileData = handle.GetRawFileData();
            s_assetDatas[asset] = fileData;
            Debug.Log($"dll名称是:{asset}  size大小为:{fileData.Length}");
        }

        
        //执行热更新代码
        yield return null;
        RawFileOperationHandle handleHotUpdate = package.LoadRawFileAsync("HotUpdate.dll");
        yield return handleHotUpdate;
        byte[] fileDataHotUpdate = handleHotUpdate.GetRawFileData();
        Debug.Log($"dll名称是:{"HotUpdate.dll"}  size大小为:{fileDataHotUpdate.Length}");
        _hotUpdateAss = Assembly.Load(fileDataHotUpdate);
        Type entryType = _hotUpdateAss.GetType("InitGame");
        entryType.GetMethod("Init").Invoke(null, null);
        GameObject.Destroy(goTemp.gameObject);
        #endregion

    }              

    //回调
    private void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        //Debug.Log(string.Format("文件总数：{0}, 已下载文件数：{1}, 下载总大小：{2}, 已下载大小：{3}", totalDownloadCount, currentDownloadCount, totalDownloadBytes, currentDownloadBytes));
        //LoadingText.text = string.Format($"文件总数：{totalDownloadCount}, 已下载文件数：{currentDownloadCount}, 下载总大小：{GetMB(totalDownloadBytes)}, 已下载大小：{GetMB(currentDownloadBytes)}");

        LoadingText.text = string.Format($"已下载文件数：{currentDownloadCount}/{totalDownloadCount}," +
            $"已下载大小：{(currentDownloadBytes)}/{(totalDownloadBytes)}");
        Debug.Log("需要下载的:" + currentDownloadBytes);
    }//更新中
    private void OnDownloadErrorFunction(string fileName, string error)
    {
        //Debug.LogError(string.Format("下载出错：文件名：{0}, 错误信息：{1}", fileName, error));
        LoadingText.text = string.Format($"下载出错：文件名：{fileName}, 错误信息：{error}");
    }//下载出错
    private void OnStartDownloadFileFunction(string fileName, long sizeBytes)
    {
        //Debug.Log(string.Format("开始下载：文件名：{0}, 文件大小：{1}", fileName, sizeBytes));
        //LoadingText.text = string.Format($"开始下载：文件名：{fileName}, 文件大小：{GetMB(sizeBytes)}");
        LoadingText.text = string.Format($"文件大小：{GetMB(sizeBytes)}");
    }//开始下载
    private void OnDownloadOverFunction(bool isSucceed)
    {
        //Debug.Log("下载" + (isSucceed ? "成功" : "失败"));
        LoadingText.text = $"下载{(isSucceed ? "成功" : "失败")}";
    }                   //下载完成
    private void OnClearCacheFunction(AsyncOperationBase asyncOperationBase)
    {
        //Debug.Log("清理缓存完毕,流程更新完毕");
        LoadingText.text = "清理缓存完毕,流程更新完毕";
    }//清理缓存

    //辅助功能
    IEnumerator LoadAsset(string LoadAssetPath, string SaveAssetPath)
    {
        yield return null;

        //下载文件
        UnityWebRequest huwr = UnityWebRequest.Head(LoadAssetPath);
        yield return huwr.SendWebRequest();
        if (huwr.result == UnityWebRequest.Result.ConnectionError || huwr.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError($"请求的链接错误{huwr.error}"); //出现错误 输出错误信息
            FsmProcessChange(EProcess.FsmErrorPrepare);
            yield break;
        }
        long totalLength = long.Parse(huwr.GetResponseHeader("Content-Length")); //首先拿到文件的全部长度
        string dirPath = Path.GetDirectoryName(SaveAssetPath);//获取文件的上一级目录

        Directory.Delete(dirPath, true);
        if (!Directory.Exists(dirPath)) //判断路径是否存在
            Directory.CreateDirectory(dirPath);//不存在创建

        /*作用：创建一个文件流，指定路径为filePath,模式为打开或创建，访问为写入
            * 使用using(){}方法原因： 当同一个cs引用了不同的命名空间，但这些命名控件都包括了一个相同名字的类型的时候,可以使用using关键字来创建别名，这样会使代码更简洁。注意：并不是说两名字重复，给其中一个用了别名，另外一个就不需要用别名了，如果两个都要使用，则两个都需要用using来定义别名的
            * using(类){} 括号中的类必须是继承了IDisposable接口才能使用否则报错
            * 这里没有出现不同命名空间出现相同名字的类属性可以不用using(){}
            */
        using (FileStream fs = new FileStream(SaveAssetPath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            long nowFileLength = fs.Length; //当前文件长度,断点前已经下载的文件长度。
            Debug.Log(fs.Length);
            //判断当前文件是否小于要下载文件的长度，即文件是否下载完成
            if (nowFileLength < totalLength)
            {
                Debug.Log("还没下载完成");
                /*使用Seek方法 可以随机读写文件
                * Seek()  ----------有两个参数 第一参数规定文件指针以字节为单位移动的距离。第二个参数规定开始计算的位置
                * 第二个参数SeekOrigin 有三个值：Begin  Current   End
                * fs.Seek(8,SeekOrigin.Begin);表示 将文件指针从开头位置移动到文件的第8个字节
                * fs.Seek(8,SeekOrigin.Current);表示 将文件指针从当前位置移动到文件的第8个字节
                * fs.Seek(8,SeekOrigin.End);表示 将文件指针从最后位置移动到文件的第8个字节
                */
                fs.Seek(nowFileLength, SeekOrigin.Begin);  //从开头位置，移动到当前已下载的子节位置
                UnityWebRequest uwr = UnityWebRequest.Get(LoadAssetPath); //创建UnityWebRequest对象，将Url传入
                uwr.SetRequestHeader("Range", "bytes=" + nowFileLength + "-" + totalLength);//修改请求头从n-m之间
                uwr.SendWebRequest();                      //开始请求
                if (huwr.result == UnityWebRequest.Result.ConnectionError || huwr.result == UnityWebRequest.Result.ProtocolError) //如果出错
                {
                    Debug.Log(uwr.error); //输出 错误信息
                    yield break;
                }

                long index = 0;     //从该索引处继续下载
                while (nowFileLength < totalLength) //只要下载没有完成，一直执行此循环
                {
                    yield return null;
                    byte[] data = uwr.downloadHandler.data;
                    if (data != null)
                    {
                        long length = data.Length - index;
                        fs.Write(data, (int)index, (int)length); //写入文件
                        index += length;
                        nowFileLength += length;
                        Debug.Log($"当前进度是:{Math.Floor((float)nowFileLength / totalLength * 100)}%");
                        if (nowFileLength >= totalLength) //如果下载完成了
                        {
                            Debug.Log("下载完成");
                            break;
                        }
                    }
                }
                huwr.Dispose();
                uwr.Dispose();
            }
        }
    }//下载资源功能
    private string GetHostServerURL()
    {
        //加载XML文件信息
        XmlDocument xml = new XmlDocument();
        //加载
        //xml.Load(SaveXMLVersion);
        //读取数据
        //XmlNode root = xml.SelectSingleNode("PackageInfo");
        //XmlNode nodeItem = root.SelectSingleNode("URL");
        //Debug.Log($"获取的PackageURL是{nodeItem.Attributes["PackageURL"].Value}");
        //Debug.Log($"获取的Verson是{nodeItem.Attributes["Verson"].Value}");

        //TODO 后续会改成XML读取配置
        //string hostServerIP = "http://127.0.0.1:8000/"; //安卓模拟器地址
        string hostServerIP = "http://192.168.2.4:8000/"; //安卓模拟器地址
        string appVersion = "1"; //安卓模拟器地址
        //string hostServerIP = nodeItem.Attributes["PackageURL"].Value;
        //string appVersion = nodeItem.Attributes["Verson"].Value;

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{hostServerIP}/Android/Android/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{hostServerIP}/StandaloneWindows64/IPhone/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{hostServerIP}/StandaloneWindows64/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/StandaloneWindows64/PC/{appVersion}";//需要添加http开头的
        //return $"{hostServerIP}/CDN/PC/{appVersion}";

#else
		if (Application.platform == RuntimePlatform.Android)
			return $"{hostServerIP}/CDN/Android/{appVersion}";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return $"{hostServerIP}/CDN/IPhone/{appVersion}";
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
			return $"{hostServerIP}/CDN/WebGL/{appVersion}";
		else
			return $"{hostServerIP}/StandaloneWindows64/PC/{appVersion}";
#endif

    }//获取资源服务器地址
    private static void LoadMetadataForAOTAssemblies()
    {
        //  为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
        //  一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
        //  注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        //  热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyNames)
        {
            byte[] dllBytes = GetAssetData(aotDllName);
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }//补充元数据
    private static byte[] GetAssetData(string dllName)
    {
        return s_assetDatas[dllName];
    }//获取byte字节数组
    private string GetMB(long b)
    {
        for (int i = 0; i < 2; i++)
            b /= 1024;
        return $"{b}MB";
    }//将B转换为MB

}
