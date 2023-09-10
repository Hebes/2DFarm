using ACFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    单个NPC的控制

-----------------------*/

namespace ACFarm
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class NPCMovement : MonoBehaviour, ISaveable
    {
        public List<ScheduleDetails> scheduleList;
        private SortedSet<ScheduleDetails> scheduleSet;         //排序的Schedule
        private ScheduleDetails currentSchedule;                //当前的Schedule

        //临时存储信息
        public string NPCName;                                  //NPC名称(这个从Excel复制)
        public string currentScene;                             //当前场景
        private string targetScene;                             //目标场景
        private Vector3Int currentGridPosition;                 //当前场景位置
        private Vector3Int tragetGridPosition;                  //目标场景位置
        private Vector3Int nextGridPosition;                    //下一个位置
        private Vector3 nextWorldPosition;                      //下一个世界坐标位置

        [Header("移动属性")]
        public float normalSpeed = 2f;
        private float minSpeed = 1;
        private float maxSpeed = 3;
        private Vector2 dir;
        public bool isMoving;

        //Components
        private Rigidbody2D rb;
        private BoxCollider2D coll;
        private Animator anim;
        private Grid gird;
        private Stack<MovementStep> movementSteps;
        private Coroutine npcMoveRoutine;

        private bool isInitialised;                             //是否初始化
        private bool npcMove;                                   //NPC是否移动
        private bool sceneLoaded;                               //场景是否加载完毕,加载完毕后NPC才能移动
        public bool interactable;                               //是否可交互
        public bool isFirstLoad;                                //是否是第一次加载
        private ESeason currentSeason;                          //当前季节
        //动画计时器
        private float animationBreakTime;
        private bool canPlayStopAnimaiton;
        private AnimationClip stopAnimationClip;
        public AnimationClip blankAnimationClip;
        private AnimatorOverrideController animOverride;

        private TimeSpan GameTime => TimeManagerSystem.Instance.GameTime;




        //生命周期
        private void Awake()
        {
            //获取组件
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();

            movementSteps = new Stack<MovementStep>();
            scheduleSet = new SortedSet<ScheduleDetails>();
            //运行时的动画控制器
            animOverride = new AnimatorOverrideController(anim.runtimeAnimatorController);
            anim.runtimeAnimatorController = animOverride;

            List<ScheduleDetailsData> scheduleDetailsDatas = DataExpansion.GetDataList<ScheduleDetailsData>();
            foreach (ScheduleDetailsData item in scheduleDetailsDatas)
            {
                if (!item.NPCName.Equals(NPCName)) continue;
                Vector2Int vector2IntTemp = new Vector2Int(item.targetGridPositionX, item.targetGridPositionY);
                AnimationClip animationClipTemp = item.clipAtStop.Equals("null") ? null : YooAssetLoadExpsion.YooaddetLoadSync<AnimationClip>(item.clipAtStop);
                ScheduleDetails scheduleDetails = new ScheduleDetails(
                    item.hour,
                    item.minute,
                    item.day,
                    item.priority,
                    (ESeason)item.Season,
                    item.targetScene,
                    vector2IntTemp,
                    animationClipTemp,
                    item.interactable);

                scheduleSet.Add(scheduleDetails);
            }

            //注册保存事件
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }
        private void OnEnable()
        {
            ConfigEvent.AfterSceneLoadedEvent.AddEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.BeforeSceneUnloadEvent.AddEventListener(OnBeforeSceneUnloadEvent);

            ConfigEvent.GameMinute.AddEventListener<int, int, int, ESeason>(OnGameMinuteEvent);
            ConfigEvent.EndGameEvent.AddEventListener(OnEndGameEvent);
            ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
        }
        private void OnDisable()
        {
            ConfigEvent.AfterSceneLoadedEvent.RemoveEventListener(OnAfterSceneLoadedEvent);
            ConfigEvent.BeforeSceneUnloadEvent.RemoveEventListener(OnBeforeSceneUnloadEvent);

            ConfigEvent.GameMinute.RemoveEventListener<int, int, int, ESeason>(OnGameMinuteEvent);
            ConfigEvent.EndGameEvent.RemoveEventListener(OnEndGameEvent);
            ConfigEvent.StartNewGameEvent.RemoveEventListener<int>(OnStartNewGameEvent);
        }
        private void Start()
        {
            ISaveable saveable = this;
            saveable.RegisterSaveable();
        }
        private void Update()
        {
            if (sceneLoaded)
                SwitchAnimation();

            //计时器
            animationBreakTime -= Time.deltaTime;
            canPlayStopAnimaiton = animationBreakTime <= 0;
        }
        private void FixedUpdate()
        {
            if (sceneLoaded)
                Movement();
        }



        //事件监听
        private void OnBeforeSceneUnloadEvent()
        {
            sceneLoaded = false;
        }
        private void OnAfterSceneLoadedEvent()
        {
            gird = FindObjectOfType<Grid>();
            CheckVisiable();

            if (!isInitialised)
            {
                InitNPC();
                isInitialised = true;
            }

            sceneLoaded = true;

            //加载记录时NPC可以继续移动
            if (!isFirstLoad)
            {
                currentGridPosition = gird.WorldToCell(transform.position);
                var schedule = new ScheduleDetails(
                    0,
                    0, 
                    0, 
                    0, 
                    currentSeason, 
                    targetScene, 
                    (Vector2Int)tragetGridPosition, 
                    stopAnimationClip, 
                    interactable);

                BuildPath(schedule);
                isFirstLoad = true;
            }
        }
        private void OnGameMinuteEvent(int minute, int hour, int day, ESeason season)
        {
            int time = (hour * 100) + minute;
            currentSeason = season;

            ScheduleDetails matchSchedule = null;
            foreach (var schedule in scheduleSet)
            {
                if (schedule.Time == time)
                {
                    if (schedule.day != day && schedule.day != 0)
                        continue;
                    if (schedule.season != season)
                        continue;
                    matchSchedule = schedule;
                }
                else if (schedule.Time > time)
                {
                    break;
                }
            }
            if (matchSchedule != null)
                BuildPath(matchSchedule);
        }
        private void OnEndGameEvent()
        {
            sceneLoaded = false;
            npcMove = false;
            if (npcMoveRoutine != null)
                StopCoroutine(npcMoveRoutine);
        }
        private void OnStartNewGameEvent(int obj)
        {
            isInitialised = false;
            isFirstLoad = true;
        }
        private void InitNPC()
        {
            targetScene = currentScene;

            //保持在当前坐标的网格中心点
            currentGridPosition = gird.WorldToCell(transform.position);
            transform.position = new Vector3(currentGridPosition.x + ConfigSettings.gridCellSize / 2f, currentGridPosition.y + ConfigSettings.gridCellSize / 2f, 0);

            tragetGridPosition = currentGridPosition;
        }


        //NPC是否看的见
        /// <summary>
        /// 检查NPC是否看的见
        /// </summary>
        private void CheckVisiable()
        {
            SetActiveInScene(currentScene == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
        /// <summary>
        /// 设置NPC显示情况
        /// </summary>
        /// <param name="isActive">是否活动</param>
        private void SetActiveInScene(bool isActive)
        {
            transform.GetSpriteRenderer().enabled = isActive;
            coll.enabled = isActive;

            transform.GetChild(0).gameObject.SetActive(isActive);
        }
        /// <summary>
        /// 主要移动方法
        /// </summary>
        private void Movement()
        {
            if (npcMove) return;
            if (movementSteps.Count > 0)
            {
                MovementStep step = movementSteps.Pop();


                currentScene = step.sceneName;

                CheckVisiable();

                nextGridPosition = (Vector3Int)step.gridCoordinate;
                TimeSpan stepTime = new TimeSpan(step.hour, step.minute, step.second);

                MoveToGridPosition(nextGridPosition, stepTime);
            }
            else if (!isMoving && canPlayStopAnimaiton)
            {
                StartCoroutine(SetStopAnimation());
            }
        }
        private void MoveToGridPosition(Vector3Int gridPos, TimeSpan stepTime)
        {
            npcMoveRoutine = StartCoroutine(MoveRoutine(gridPos, stepTime));
        }
        private IEnumerator MoveRoutine(Vector3Int gridPos, TimeSpan stepTime)
        {
            npcMove = true;
            nextWorldPosition = GetWorldPostion(gridPos);

            //还有时间用来移动
            if (stepTime > GameTime)
            {
                //用来移动的时间差，以秒为单位
                float timeToMove = (float)(stepTime.TotalSeconds - GameTime.TotalSeconds);
                //实际移动距离
                float distance = Vector3.Distance(transform.position, nextWorldPosition);
                //实际移动速度
                float speed = Mathf.Max(minSpeed, (distance / timeToMove / ConfigSettings.secondThreshold));

                if (speed <= maxSpeed)
                {
                    while (Vector3.Distance(transform.position, nextWorldPosition) > ConfigSettings.pixelSize)//已经到了 
                    {
                        dir = (nextWorldPosition - transform.position).normalized;//方向

                        Vector2 posOffset = new Vector2(dir.x * speed * Time.fixedDeltaTime, dir.y * speed * Time.fixedDeltaTime);
                        rb.MovePosition(rb.position + posOffset);
                        yield return new WaitForFixedUpdate();
                    }
                }
            }
            //如果时间已经到了就瞬移
            rb.position = nextWorldPosition;
            currentGridPosition = gridPos;
            nextGridPosition = currentGridPosition;

            npcMove = false;
        }


        /// <summary>
        /// 根据Schedule构建路径
        /// </summary>
        /// <param name="schedule"></param>
        public void BuildPath(ScheduleDetails schedule)
        {
            movementSteps.Clear();
            currentSchedule = schedule;
            targetScene = schedule.targetScene;
            tragetGridPosition = (Vector3Int)schedule.targetGridPosition;
            stopAnimationClip = schedule.clipAtStop;
            this.interactable = schedule.interactable;

            if (schedule.targetScene == currentScene)
            {
                AStar.Instance.BuildPath(schedule.targetScene, (Vector2Int)currentGridPosition, schedule.targetGridPosition, movementSteps);
            }
            else if (schedule.targetScene != currentScene)//跨场景移动
            {
                SceneRoute sceneRoute = NPCManagerSystem.Instance.GetSceneRoute(currentScene, schedule.targetScene);//获得两个场景间的路径

                for (int i = 0; i < sceneRoute?.scenePathList.Count; i++)
                {
                    Vector2Int fromPos, gotoPos;
                    ScenePath path = sceneRoute.scenePathList[i];

                    fromPos = path.fromGridCell.x >= ConfigSettings.maxGridSize ? (Vector2Int)currentGridPosition : path.fromGridCell;
                    gotoPos = path.gotoGridCell.x >= ConfigSettings.maxGridSize ? schedule.targetGridPosition : path.gotoGridCell;

                    AStar.Instance.BuildPath(path.sceneName, fromPos, gotoPos, movementSteps);
                }
            }

            if (movementSteps.Count > 1)
            {
                //更新每一步对应的时间戳
                UpdateTimeOnPath();
            }
        }

        /// <summary>
        /// 更新路径上每一步的时间
        /// </summary>
        private void UpdateTimeOnPath()
        {
            MovementStep previousSetp = null;

            TimeSpan currentGameTime = GameTime;

            foreach (MovementStep step in movementSteps)
            {
                if (previousSetp == null)
                    previousSetp = step;

                step.hour = currentGameTime.Hours;
                step.minute = currentGameTime.Minutes;
                step.second = currentGameTime.Seconds;

                TimeSpan gridMovementStepTime;

                if (MoveInDiagonal(step, previousSetp))
                    gridMovementStepTime = new TimeSpan(0, 0, (int)(ConfigSettings.gridCellDiagonalSize / normalSpeed / ConfigSettings.secondThreshold));
                else
                    gridMovementStepTime = new TimeSpan(0, 0, (int)(ConfigSettings.gridCellSize / normalSpeed / ConfigSettings.secondThreshold));

                //累加获得下一步的时间戳
                currentGameTime = currentGameTime.Add(gridMovementStepTime);
                //循环下一步
                previousSetp = step;
            }
        }

        /// <summary>
        /// 判断是否走斜方向
        /// </summary>
        /// <param name="currentStep"></param>
        /// <param name="previousStep"></param>
        /// <returns></returns>
        private bool MoveInDiagonal(MovementStep currentStep, MovementStep previousStep)
        {
            return (currentStep.gridCoordinate.x != previousStep.gridCoordinate.x) && (currentStep.gridCoordinate.y != previousStep.gridCoordinate.y);
        }

        /// <summary>
        /// 网格坐标返回世界坐标中心点
        /// </summary>
        /// <param name="gridPos"></param>
        /// <returns></returns>
        private Vector3 GetWorldPostion(Vector3Int gridPos)
        {
            Vector3 worldPos = gird.CellToWorld(gridPos);
            return new Vector3(worldPos.x + ConfigSettings.gridCellSize / 2f, worldPos.y + ConfigSettings.gridCellSize / 2);
        }

        /// <summary>
        /// 切换动画
        /// </summary>
        private void SwitchAnimation()
        {
            isMoving = transform.position != GetWorldPostion(tragetGridPosition);

            anim.SetBool("IsMoving", isMoving);
            if (isMoving)
            {
                anim.SetBool("Exit", true);
                anim.SetFloat("DirX", dir.x);
                anim.SetFloat("DirY", dir.y);
            }
            else
            {
                anim.SetBool("Exit", false);
            }
        }

        private IEnumerator SetStopAnimation()
        {
            //强制面向镜头
            anim.SetFloat("DirX", 0);
            anim.SetFloat("DirY", -1);

            animationBreakTime = ConfigSettings.animationBreakTime;
            if (stopAnimationClip != null)
            {
                animOverride[blankAnimationClip] = stopAnimationClip;
                anim.SetBool("EventAnimation", true);
                yield return null;
                anim.SetBool("EventAnimation", false);
            }
            else
            {
                animOverride[stopAnimationClip] = blankAnimationClip;
                anim.SetBool("EventAnimation", false);
            }
        }



        //保存数据
        public string GUID => NPCName;
        public GameSaveData GenerateSaveData()
        {
            GameSaveData saveData = new GameSaveData();
            saveData.characterPosDict = new Dictionary<string, SerializableVector3>();
            saveData.characterPosDict.Add("targetGridPosition", new SerializableVector3(tragetGridPosition));
            saveData.characterPosDict.Add("currentPosition", new SerializableVector3(transform.position));
            saveData.dataSceneName = currentScene;
            saveData.targetScene = this.targetScene;
            if (stopAnimationClip != null)
            {
                saveData.animationInstanceID = stopAnimationClip.GetInstanceID();
            }
            saveData.interactable = this.interactable;
            saveData.timeDict = new Dictionary<string, int>();
            saveData.timeDict.Add("currentSeason", (int)currentSeason);

            return saveData;
        }
        public void RestoreData(GameSaveData saveData)
        {
            isInitialised = true;
            isFirstLoad = false;

            currentScene = saveData.dataSceneName;
            targetScene = saveData.targetScene;

            Vector3 pos = saveData.characterPosDict["currentPosition"].ToVector3();
            Vector3Int gridPos = (Vector3Int)saveData.characterPosDict["targetGridPosition"].ToVector2Int();

            transform.position = pos;
            tragetGridPosition = gridPos;

            if (saveData.animationInstanceID != 0)
            {
                this.stopAnimationClip = Resources.InstanceIDToObject(saveData.animationInstanceID) as AnimationClip;
            }

            this.interactable = saveData.interactable;
            this.currentSeason = (ESeason)saveData.timeDict["currentSeason"];
        }
    }
}
