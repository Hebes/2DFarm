using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Graphs;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    存储箱子

-----------------------*/

namespace ACFrameworkCore
{
    public class Box : MonoBehaviour
    {
        public List<InventoryItem> boxBagData;          //箱包数据

        public SpriteRenderer mouseIcon;
        private bool canOpen = false;
        private bool isOpen;
        public int index;
        public string boxName;                          //箱子名称

        private void Awake()
        {
            mouseIcon = this.GetChildComponent<SpriteRenderer>("Sign");
            InitBox(1);
        }

        private void OnEnable()
        {
            if (boxBagData == null)
                boxBagData = new List<InventoryItem>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(ConfigTag.TagPlayer))
            {
                canOpen = true;
                mouseIcon.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(ConfigTag.TagPlayer))
            {
                canOpen = false;
                mouseIcon.enabled = false;
            }
        }

        private void Update()
        {
            if (!isOpen && canOpen && Input.GetMouseButtonDown(1))
            {
                //打开箱子
                ConfigEvent.BaseBagOpen.EventTrigger(boxName, string.Empty);
                isOpen = true;
            }

            if (!canOpen && isOpen)
            {
                //关闭箱子
                ConfigEvent.BaseBagClose.EventTrigger(boxName, string.Empty);
                isOpen = false;
            }

            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                //关闭箱子
                ConfigEvent.BaseBagClose.EventTrigger(boxName, string.Empty);
                isOpen = false;
            }
        }

        /// <summary>
        /// 初始化Box和数据
        /// </summary>
        /// <param name="boxIndex"></param>
        public void InitBox(int boxIndex)
        {
            index = boxIndex;
            var key = this.name + index;
            if (InventoryAllSystem.Instance.ItemDicArray.ContainsKey(boxName))  //刷新地图读取数据
            {
                boxBagData = InventoryAllSystem.Instance.GetItemListArray(boxName).ToList();
            }
            else     //新建箱子
            {
                InventoryAllSystem.Instance.ItemDicArray.Add(boxName, new InventoryItem[10]);

                InventoryAllSystem.Instance.ItemDicArray[boxName][0] = new InventoryItem() { itemID = 1007, itemAmount = 10 };
                InventoryAllSystem.Instance.ItemDicArray[boxName][1] = new InventoryItem() { itemID = 1008, itemAmount = 10 };
                InventoryAllSystem.Instance.ItemDicArray[boxName][2] = new InventoryItem() { itemID = 1009, itemAmount = 10 };
                InventoryAllSystem.Instance.ItemDicArray[boxName][3] = new InventoryItem() { itemID = 1010, itemAmount = 10 };
            }
        }
    }
}
