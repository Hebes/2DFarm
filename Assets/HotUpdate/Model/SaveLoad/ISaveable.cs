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
        string GUID { get; }
        void RegisterSaveable()
        {
            SaveLoadManagerSystem.Instance.RegisterSaveable(this);
        }
        GameSaveData GenerateSaveData();
        void RestoreData(GameSaveData saveData);
    }
}
