using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACFrameworkCore
{
    public class UnityLoad : Iload
    {
        public void Load(string path)
        {
            Resources.Load(path);
        }

        public void LoadAll(string path)
        {
            Resources.LoadAll(path);
        }
    }
}
