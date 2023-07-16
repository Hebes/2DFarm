using System.Collections.Generic;
using UnityEngine;

namespace ACFrameworkCore
{
    public class BaseUI
    {
        public GameObject UIGo { get; set; }

        /// <summary>
        /// 等价于Start
        /// </summary>
        public virtual void StartUI()
        {

        }
        /// <summary>
        /// 等价Awake
        /// </summary>
        public virtual void OpenUI()
        {

        }
        public virtual void HideUI()
        {

        }
        public virtual void RemoveUI()
        {
        }
    }
}
