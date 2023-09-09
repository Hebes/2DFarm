using System.Collections.Generic;


/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    数据保存

-----------------------*/

namespace ACFrameworkCore
{
    [System.Serializable]
    public class GameSaveData
    {
        public string dataSceneName;
        //存储人物坐标，string人物名字
        public Dictionary<string, SerializableVector3> characterPosDict;
        //存储场景物品
        public Dictionary<string, List<SceneItem>> sceneItemDict;
        //记录场景家具
        public Dictionary<string, List<SceneFurniture>> sceneFurnitureDict;
        //地图格子信息
        public Dictionary<string, TileDetails> tileDetailsDict;
        //是否是第一次加载
        public Dictionary<string, bool> firstLoadDict;
        //物品数据
        public Dictionary<string, List<InventoryItem>> inventoryDict;
        public Dictionary<string, InventoryItem[]> ItemDicArray; //物品字典列表   两本字典的KEY请不要重复

        //时间
        public Dictionary<string, int> timeDict;
        //玩家金币
        public int playerMoney;

        //NPC
        public string targetScene;
        public bool interactable;
        public int animationInstanceID;
    }
}
