using UnityEngine;
using Farm2D;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    玩家碰撞拾取

-----------------------*/

namespace Farm2D
{
    public class PlayerItemPickUP : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Item item = collision.GetComponent<Item>();
            if (item == null) return;                                       //是否为空
            if (item.itemDetails.canPickedup == false) return;              //能否被拾取
            bool isAddOK = ModelItem.Instance.AddItem(ConfigEvent.ActionBar, item.itemID, item.itemAmount);//拾取物品到背包
            if (isAddOK)
                GameObject.Destroy(item.gameObject);
        }
    }
}
