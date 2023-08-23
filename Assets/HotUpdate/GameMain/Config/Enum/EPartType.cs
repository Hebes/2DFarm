
namespace ACFrameworkCore
{
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
        /// <summary> 举东西的状态 </summary>
        Carry = 1,
        /// <summary> 锄头 </summary>
        Hoe = 2,
        /// <summary> 打碎 </summary>
        Break = 3,
        /// <summary> 砍 </summary>
        Chop = 4,
        Reap = 5,
        Water = 6,
        Collect = 7,
    }
}
