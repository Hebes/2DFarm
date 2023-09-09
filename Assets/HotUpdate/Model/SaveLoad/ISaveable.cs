using ACFrameworkCore;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    保存数据接口

-----------------------*/

namespace ACFarm
{
    public interface ISaveable
    {
        public string GUID { get; }
        /// <summary>
        /// 注册保存
        /// </summary>
        public void RegisterSaveable()
        {
            SaveLoadManagerSystem.Instance.RegisterSaveable(this);
        }
        /// <summary>
        /// 生成数据
        /// </summary>
        /// <returns></returns>
        public GameSaveData GenerateSaveData();
        /// <summary>
        /// 回复数据
        /// </summary>
        /// <param name="saveData"></param>
        public void RestoreData(GameSaveData saveData);
    }
}
