using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACFarm
{
    public class CommonData : MonoBehaviour
    {
        public static CommonData Instance;
        public CropDataList_SO cropDataList_SO;

        private void Awake()
        {
            Instance = this;
        }
    }
}
