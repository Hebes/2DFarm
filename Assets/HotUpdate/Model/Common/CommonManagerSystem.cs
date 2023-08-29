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
        private Player player;                      //玩家
        private Camera camera;                      //主摄像机

        public void ICroeInit()
        {
            Instance = this;
        }

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
    }
}
