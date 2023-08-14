/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    物品的枚举

-----------------------*/

namespace ACFrameworkCore
{
    public enum EItemType
    {
        /// <summary>种子</summary>
        Seed = 1,
        /// <summary> 商品 </summary>
        Commdity = 2,
        /// <summary> 家具 </summary>
        Furniture = 3,
        /// <summary> 锄头 </summary>
        HoeTool = 4,
        /// <summary> 砍树工具 </summary>
        ChopTool = 5,
        /// <summary> 砸石头工具 </summary>
        BreakTool = 6,
        /// <summary> 割草工具 </summary>
        ReapTool = 7,
        /// <summary> 浇水工具 </summary>
        WaterTool = 8,
        /// <summary> 菜篮子收割工具 </summary>
        ClooectTool = 9,
        /// <summary> 可以被割的杂草 </summary>
        ReapableSceney = 10,
    }

    

    /// <summary>
    /// 物品的位置
    /// </summary>
    public enum EInventoryLocation
    {
        Player = 1,
        Box = 2,
    }

    /// <summary>
    /// 物体类型类型
    /// </summary>
    public enum EPartType
    {
        None = 0,
        /// <summary>
        /// 举东西的状态
        /// </summary>
        Carry = 1,
        /// <summary>
        /// 锄头
        /// </summary>
        Hoe = 2,
        Break = 3,
    }

}
