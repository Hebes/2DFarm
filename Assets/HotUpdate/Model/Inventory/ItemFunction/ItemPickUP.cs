using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    玩家拾取

-----------------------*/

namespace ACFrameworkCore
{
    public class ItemPickUP :MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                if (item.itemDatails.canPickedup)
                {
                    //拾取物品到背包
                    InventoryAllSystem.Instance.AddItemDicArray(ConfigInventory.ActionBar,item, true);
                }
            }
        }
    }
}
