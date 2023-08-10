/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    工具栏

-----------------------*/

using System.Linq;
using UnityEngine;

namespace ACFrameworkCore
{
    public class ActionBarPanel : UIBase
    {
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            ACUIComponent aCUIComponent = panelGameObject.GetComponent<ACUIComponent>();
            //aCUIComponent
            InventoryItem[] inventoryItems = InventoryAllManager.Instance.ItemDicArray[ConfigInventory.ActionBar];
            GameObject gameObject = ResourceExtension.LoadAsyncAsT<GameObject>(ConfigPrefab.ItemBasePrefab);

            for (int i = 0; i < inventoryItems.Length; i++)
            {
                GameObject go = GameObject.Instantiate(gameObject);
                Item item = go.GetComponent<Item>();
                item.Init(inventoryItems[i].itemID);
            }

            //InventoryAllManager.Instance.AddItem
            //RigisterButtonObjectEvent("Back",
            //  p =>
            //  {
            //      CloseUIForm();
            //  });
        }
    }
}
