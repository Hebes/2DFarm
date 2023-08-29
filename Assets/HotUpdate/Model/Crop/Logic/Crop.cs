using System.Collections;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    场景的成长的农作物

-----------------------*/

namespace ACFrameworkCore
{
    public class Crop : MonoBehaviour
    {
        public CropDetails cropDetails;
        public TileDetails tileDetails;
        private int harvestActionCount;
        public bool CanHarvest => tileDetails.growthDays >= cropDetails.TotalGrowthDays;

        private Animator anim;

        private Transform PlayerTransform => FindObjectOfType<Player>().transform;

        public void ProcessToolAction(ItemDetailsData tool, TileDetails tile)
        {
            tileDetails = tile;

            //工具使用次数
            int requireActionCount = cropDetails.GetTotalRequireCount(tool.itemID);
            if (requireActionCount == -1) return;

            anim = GetComponentInChildren<Animator>();

            //点击计数器
            if (harvestActionCount < requireActionCount)
            {
                harvestActionCount++;

                //判断是否有动画 树木
                if (anim != null && cropDetails.hasAnimation)
                {
                    if (PlayerTransform.position.x < transform.position.x)
                        anim.SetTrigger("RotateRight");
                    else
                        anim.SetTrigger("RotateLeft");
                }
                //播放粒子
                if (cropDetails.hasParticalEffect)
                    ConfigEvent.ParticleEffect.EventTrigger(cropDetails.effectType, transform.position + cropDetails.effectPos);
                //播放声音
                if (cropDetails.soundEffect != ESoundName.none)
                    ConfigEvent.PlaySound.EventTrigger(cropDetails.soundEffect);
            }

            if (harvestActionCount >= requireActionCount)
            {
                if (cropDetails.generateAtPlayerPosition || !cropDetails.hasAnimation)
                {
                    //生成农作物
                    ACDebug.Log($"生成农作物");
                    SpawnHarvestItems();
                }
                else if (cropDetails.hasAnimation)
                {
                    if (PlayerTransform.position.x < transform.position.x)
                        anim.SetTrigger("FallingRight");
                    else
                        anim.SetTrigger("FallingLeft");
                    ConfigEvent.PlaySound.EventTrigger(ESoundName.TreeFalling);
                    StartCoroutine(HarvestAfterAnimation());
                }
            }
        }

        private IEnumerator HarvestAfterAnimation()
        {
            while (!anim.GetCurrentAnimatorStateInfo(0).IsName("END"))
            {
                yield return null;
            }

            SpawnHarvestItems();
            //转换新物体
            if (cropDetails.transferItemID > 0)
                CreateTransferCrop();
        }

        private void CreateTransferCrop()
        {
            tileDetails.seedItemID = cropDetails.transferItemID;
            tileDetails.daysSinceLastHarvest = -1;
            tileDetails.growthDays = 0;
            ConfigEvent.RefreshCurrentMap.EventTrigger();
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
                        ConfigEvent.HarvestAtPlayerPosition.EventTrigger(ConfigInventory.ActionBar, cropDetails.producedItemID[i]);
                    }
                    else    //世界地图上生成物品
                    {
                        //判断应该生成的物品方向
                        var dirX = transform.position.x > PlayerTransform.position.x ? 1 : -1;
                        //一定范围内的随机
                        var spawnPos = new Vector3(transform.position.x + Random.Range(dirX, cropDetails.spawnRadius.x * dirX),
                        transform.position.y + Random.Range(-cropDetails.spawnRadius.y, cropDetails.spawnRadius.y), 0);
                        ConfigEvent.InstantiateItemInScene.EventTrigger(cropDetails.producedItemID[i],1, spawnPos);
                    }
                }
            }

            if (tileDetails != null)
            {
                tileDetails.daysSinceLastHarvest++;

                //是否可以重复生长
                if (cropDetails.daysToRegrow > 0 && tileDetails.daysSinceLastHarvest < cropDetails.regrowTimes - 1)
                {
                    tileDetails.growthDays = cropDetails.TotalGrowthDays - cropDetails.daysToRegrow;
                    //刷新种子
                    ConfigEvent.RefreshCurrentMap.EventTrigger();
                }
                else    //不可重复生长
                {
                    tileDetails.daysSinceLastHarvest = -1;
                    tileDetails.seedItemID = -1;
                    //FIXME:自己设计 是否吧地直接设置需要挖坑 
                    // tileDetails.daysSinceDug = -1;
                }

                Destroy(gameObject);
            }
        }
    }
}
