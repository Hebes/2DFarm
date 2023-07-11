using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACFrameworkCore
{
    public interface IRead
    {
        void SaveData(object data, string fileName);

        T LoadData<T>(string fileName) where T : class, new();
    }
}
