using System.Collections;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ACFrameworkCore
{
    //public class UnityLoad : Iload
    //{

    //    public T LoadAsset<T>(string path) where T : Object
    //    {
    //        Resources.Load<T>(path);
    //    }

    //    public void LoadAll(string path)
    //    {
    //        Resources.LoadAll(path);
    //    }

    //    public void LoadAsync(string path)
    //    {
    //        Resources.LoadAsync(path);
    //    }

    //    public void LoadAsync<T>(string path) where T : Object
    //    {
    //        Resources.LoadAsync<T>(path);
    //    }

    //    public void LoadAssetAsync<T>(string path, UnityAction<T> callback) where T : Object
    //    {
    //        MonoComponent.Instance.MonoStartCoroutine(ReallyLoadAsync(path, callback));
    //    }
    //    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    //    {
    //        ResourceRequest r = Resources.LoadAsync<T>(name);
    //        yield return r;

    //        if (r.asset is GameObject)
    //            callback(GameObject.Instantiate(r.asset) as T);
    //        else
    //            callback(r.asset as T);
    //    }
    //}
}
