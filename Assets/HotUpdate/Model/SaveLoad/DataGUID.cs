using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACFrameworkCore
{
    [ExecuteAlways]
    public class DataGUID : MonoBehaviour
    {
        public string guid;

        private void Awake()
        {
            if (guid == string.Empty)
            {
                guid = System.Guid.NewGuid().ToString();
            }
        }
    }

}
