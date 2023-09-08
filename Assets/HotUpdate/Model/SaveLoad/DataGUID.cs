using UnityEngine;

/*--------脚本描述-----------

电子邮箱：
    1607388033@qq.com
作者:
    暗沉
描述:
    存档生成标识

-----------------------*/

namespace ACFrameworkCore
{
    [ExecuteAlways]
    public class DataGUID : MonoBehaviour
    {
        public string guid;

        private void Awake()
        {
            if (guid == string.Empty)
                guid = System.Guid.NewGuid().ToString();
        }
    }
}
