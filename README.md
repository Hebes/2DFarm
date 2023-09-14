# 2DFarm

2d农场项目 unity2022.3.7f1国际版本

接入YooAsset HybridCLR Unitask替代协程

HybridCLR 热更现在暂时直接采用加载程序集方式实现

## 项目运行(如果不能报错的时候解决办法)

1. HybridCLR https://github.com/focus-creative-games/hybridclr_unity.git
2. UniTask https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask
3. YooAsset
4. 2D 一整套 feature里面的2D包含的所有的2D开发需要
5. Input System
6. Cinemachine
7.Universal RP

## 效果图

![Alt text](1.png)

![Alt text](2.png)

![Alt text](3.png)

![Alt text](4.png)

## 接入简单的框架

1. **事件中心模块**->EventManager
2. **对象池模块**->PoolManager
3. **Debug模块**->DebugManagerEventManager
4. **数据模块**->DataManager(数据通过byte文件加载)
5. **资源加载模块**->ResourceManager(有一个资源加载接口，和实际实现的YooAsset加载方法，里面封装的YooAsset资源加载模式,等待资源加载采用unitask)
6. **场景切换模块**->ACSceneManager(创建场景接口，管理类中调用接口的实际实现方法)
7. **UI模块**->UIManager(配合事件中心模块实现解耦合，全部采用监听方式执行数据传递，反向切换窗口采用栈集合实现)

## 其他子模块实现

1. **地图系统**->GridMapManagerSystem(采用的unity自带的网格地图,TilePalette画图的哪个网格数据)
2. **音频管理模块**->AudioManagerSystem
3. **建造模块**->BuildManagerSystem
4. **庄稼种植模块**->CropManagerSystem
5. **对话模块**->DialogueManagerSystem
6. **特效模块**->EffectsSystem
7. **物品管理模块**->ItemManagerSystem
8. **世界物品管理模块**->ItemWorldSystem
9. **灯管数据模块**->LightManagerSystem
10. **NPC模块**->NPCManagerSystem
11. **玩家模块**->Player
12. **数据存档和读档**->SaveLoadManagerSystem
13. **商店模块**->ShopManagerSystem
14. **时间模块**->TimeManagerSystem
15. **过场剧情模块**->TimelineManagerSystem
16. **场景切换模块**->SceneTransitionManagerSystem

以上子模块全都采用事件中心监听触发传递数据方式解耦合

## 编辑器拓展

![Alt text](5.png)

![Alt text](6.png)
