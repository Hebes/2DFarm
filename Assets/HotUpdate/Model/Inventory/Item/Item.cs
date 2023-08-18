using Cysharp.Threading.Tasks;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品信息,用于ItemBase

-----------------------*/

namespace ACFrameworkCore
{
    public class Item : MonoBehaviour
    {
        public int itemID;
        public int itemAmount;//默认数量是2
        public ItemDetails itemDatails;

        private SpriteRenderer spriteRenderer;
        private BoxCollider2D coll;

        private void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (itemID != 0)
                Init(itemID, itemAmount).Forget();
        }

        /// <summary>
        /// 根据ID生成物品
        /// </summary>
        /// <param name="ID"></param>
        public async UniTask Init(int ID,int itemAmount)
        {
            this.itemID = ID;
            this.itemDatails = InventoryAllSystem.Instance.GetItem(ID);
            this.itemAmount = itemAmount;
            if (itemDatails != null)
            {
                //加载图片
                string icon = !string.IsNullOrEmpty(itemDatails.itemOnWorldSprite) ? itemDatails.itemOnWorldSprite : itemDatails.itemIcon;
                spriteRenderer.sprite = await ResourceExtension.LoadAsyncUniTask<Sprite>(icon);
                //修改碰撞体尺寸，因为SpriteRenderer的SpriteSortPoInt修改的原因
                Vector2 newSize = new Vector2(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y);
                coll.size = newSize;//设置尺寸
                coll.offset = new Vector2(0, spriteRenderer.sprite.bounds.center.y);//设置他的偏移，根据中心点
            }
        }
    }
}
