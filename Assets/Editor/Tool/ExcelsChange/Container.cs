using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    容器类

-----------------------*/

namespace ACFrameworkCore
{
    public static class Container
    {
        /// <summary>
        /// 容器类脚本存储位置路径
        /// </summary>
        public static string DATA_CONTAINER_PATH = Application.dataPath + "/HotUpdate/GameMain/ExcelData/Container/";

        /// <summary>
        /// 生成Excel表对应的数据容器类
        /// </summary>
        /// <param name="table"></param>
        public static void GenerateExcelContainer(this DataTable table)
        {
            //得到主键索引
            int keyIndex = table.GetKeyIndex();
            //得到字段类型行
            DataRow rowType = table. GetVariableTypeRow(1);
            DATA_CONTAINER_PATH.GenerateDirectory();

            string str = "using System.Collections.Generic;\n";

            str += "public class " + table.TableName + "Container" + "\n{\n";

            str += "    ";
            str += "public Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">";
            str += "dataDic = new " + "Dictionary<" + rowType[keyIndex].ToString() + ", " + table.TableName + ">();\n";

            str += "}";

            File.WriteAllText(DATA_CONTAINER_PATH + table.TableName + "Container.cs", str);

            //刷新Project窗口
            AssetDatabase.Refresh();
        }
    }
}
