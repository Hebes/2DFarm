/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    工具栏

-----------------------*/

using UnityEngine;

namespace ACFrameworkCore
{
    public class ActionBarPanel : UIBase
    {
        public override void UIAwake()
        {
            base.UIAwake();
            InitUIBase(EUIType.Fixed, EUIMode.Normal, EUILucenyType.Pentrate);

            for (int i = 0; i < 8; i++)
            {
                GameObject gameObject= ResourceExtension.LoadAsyncAsT<GameObject>(ConfigPrefab.ItemBasePrefab);
                Item item = gameObject.GetComponent<Item>();
                
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
