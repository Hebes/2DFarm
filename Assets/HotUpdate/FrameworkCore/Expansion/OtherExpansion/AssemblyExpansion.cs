using System;
using System.Reflection;
using UnityEngine;

/*--------脚本描述-----------
				
电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
    程序集拓展类

-----------------------*/

namespace Core
{
    public static class AssemblyExpansion
    {
        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass(this string className, string namespaceName = "UnityEngine.UI")
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{className}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass<T>(this string className, string namespaceName = "UnityEngine.UI") where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass<T>(this UnityEngine.Object obj, string namespaceName = "UnityEngine.UI") where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }

        /// <summary>
        /// 反射命名空间
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="namespaceName">空间名</param>
        public static Type ACReflectClass<T>(this string namespaceName) where T : Component
        {
            Assembly assem = Assembly.Load(namespaceName);
            Type type = assem.GetType($"{namespaceName}.{typeof(T).Name}");
            return type;
        }
    }
}
