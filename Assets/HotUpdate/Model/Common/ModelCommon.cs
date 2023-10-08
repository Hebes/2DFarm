using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	公共参数

-----------------------*/


namespace Farm2D
{
    public class ModelCommon : IModelInit
    {
        public static ModelCommon Instance;
        public int playerMoney;     //玩家金币
        private Player player;      //玩家
        private Camera camera;      //主摄像机
        public Camera mainCamera
        {
            get
            {
                if (camera == null)
                    camera = Camera.main;
                return camera;
            }
        }
        public Transform playerTransform
        {
            get
            {
                if (player == null)
                    player = GameObject.FindObjectOfType<Player>();
                return player.transform;
            }
        }

        public async UniTask ModelInit()
        {
            Instance = this;
            ConfigEvent.StartNewGameEvent.EventAdd<int>(StartNewGameEvent);
            await UniTask.Yield();
        }

        private void StartNewGameEvent(int obj)
        {
            playerMoney = ConfigSettings.playerStartMoney;
        }
    }
}
