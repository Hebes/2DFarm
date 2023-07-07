using UnityEngine;

namespace ACFrameworkCore
{
    public class UnityLoad : Iload
    {
        public void Load(string path)
        {
            Resources.Load(path);
        }

        public void Load<T>(string path) where T : Object
        {
            Resources.Load<T>(path);
        }

        public void LoadAll(string path)
        {
            Resources.LoadAll(path);
        }

        public void LoadAsync(string path)
        {
            Resources.LoadAsync(path);
        }
    }
}
