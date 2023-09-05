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
        public List<DialogueDetailsData> DialogueDetailsDataList;
        public List<ShopDetailsData> ShopDetailsDataList;
        public List<BluePrintDetailsData> BluePrintDetailsDataList;
        public List<LightDetailsData> LightDetailsDataList;

        private void Awake()
        {
            ItemDetailsDataList = this.GetDataListThis<ItemDetailsData>();
            PlayerAnimatorsDataList = this.GetDataListThis<PlayerAnimatorsData>();
            ScheduleDetailsDataList = this.GetDataListThis<ScheduleDetailsData>();
            SceneRouteDetailsDataList = this.GetDataListThis<SceneRouteDetailsData>();
            DialogueDetailsDataList = this.GetDataListThis<DialogueDetailsData>();
            ShopDetailsDataList = this.GetDataListThis<ShopDetailsData>();
            BluePrintDetailsDataList = this.GetDataListThis<BluePrintDetailsData>();
            LightDetailsDataList = this.GetDataListThis<LightDetailsData>();
        }
    }
}
