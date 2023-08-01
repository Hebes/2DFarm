using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ACFrameworkCore
{
    public class test2 : IACSinglent<test2>
    {
        public void yyy()
        {
            Debug.Log("通过单例取出test2");
        }
    }
}
