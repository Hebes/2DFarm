using System.Collections.Generic;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	显示DataManager数据

-----------------------*/

namespace ACFrameworkCore
{
    public class ShowDataManager : MonoBehaviour
    {
        public List<ItemDetailsData> ItemDetailsDataList;
        public List<PlayerAnimatorsData> PlayerAnimatorsDataList;
        public List<ScheduleDetailsData> ScheduleDetailsDataList;
        public List<SceneRouteDetailsData> SceneRouteDetailsDataList;

        private void Awake()
        {
            ItemDetailsDataList = this.GetDataListT<ItemDetailsData>();
            PlayerAnimatorsDataList = this.GetDataListT<PlayerAnimatorsData>();
            ScheduleDetailsDataList = this.GetDataListT<ScheduleDetailsData>();
            SceneRouteDetailsDataList = this.GetDataListT<SceneRouteDetailsData>();
        }
    }
}
