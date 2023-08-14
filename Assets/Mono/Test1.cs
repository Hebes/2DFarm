using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ACFrameworkCore
{
    public class Test1 : MonoBehaviour
    {
        GameObject go;
        private void Start()
        {
            go = Resources.Load<GameObject>("Cube");
            GameObject.Instantiate(go);
            Invoke("Remove", 2);
        }

        public void Remove()
        {
            go = null;
        }
    }
}
