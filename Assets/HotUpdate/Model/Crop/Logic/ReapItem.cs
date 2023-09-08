using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    收割周围的杂草

-----------------------*/

namespace ACFrameworkCore
{
    public class ReapItem : MonoBehaviour
    {
        private CropDetails cropDetails;

        public void InitCropData(int ID)
        {
            cropDetails = CropManager.Instance.GetCropDetails(ID);
        }

        /// <summary>
        /// 生成果实
        /// </summary>
        public void SpawnHarvestItems()
        {
            for (int i = 0; i < cropDetails.producedItemID.Length; i++)
            {
                int amountToProduce;

                if (cropDetails.producedMinAmount[i] == cropDetails.producedMaxAmount[i])
                {
                    //代表只生成指定数量的
                    amountToProduce = cropDetails.producedMinAmount[i];
                }
                else    //物品随机数量
                {
                    amountToProduce = Random.Range(cropDetails.producedMinAmount[i], cropDetails.producedMaxAmount[i] + 1);
                }

                //执行生成指定数量的物品
                for (int j = 0; j < amountToProduce; j++)
                {
                    if (cropDetails.generateAtPlayerPosition)
                    {
                        ConfigEvent.HarvestAtPlayerPosition.EventTrigger(ConfigEvent.ActionBar, cropDetails.producedItemID[i]);
                    }
                    else    //世界地图上生成物品
                    {
                        //判断应该生成的物品方向
                        var dirX = transform.position.x > CommonManagerSystem.Instance.playerTransform.position.x ? 1 : -1;
                        //一定范围内的随机
                        var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
                        transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);

                        ConfigEvent.InstantiateItemInScene.EventTrigger(cropDetails.producedItemID[i], 1, spawnPos);
                    }
                }
            }

        }
    }
}