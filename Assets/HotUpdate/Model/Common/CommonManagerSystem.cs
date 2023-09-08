using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	公共参数

-----------------------*/


namespace ACFrameworkCore
{
    public class CommonManagerSystem : ICore
    {

        public static CommonManagerSystem Instance;
        
        public void ICroeInit()
        {
            Instance = this;
        }

        //玩家
        private Player player;                      //玩家
        private Camera camera;                      //主摄像机
        private int money = 1000;                    //玩家金币
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
        public int playerMoney
        {
            get => money;
            set => money = value;
        }

        //其他
    }
}
