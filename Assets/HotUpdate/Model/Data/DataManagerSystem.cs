using ACFrameworkCore;
using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	牧场数据初始化管理系统

-----------------------*/

namespace ACFarm
{
    public class DataManagerSystem : ICore
    {
        public static DataManagerSystem Instance;
        private DataManager dataManager;

        public void ICroeInit()
        {
            Instance = this;
            dataManager = DataManager.Instance;
        }
    }
}
