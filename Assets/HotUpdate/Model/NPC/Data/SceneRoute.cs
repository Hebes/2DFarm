using System.Collections.Generic;
using UnityEngine;

namespace Farm2D
{
    //场景路径
    [System.Serializable]
    public class SceneRoute
    {
        public string fromSceneName;
        public string gotoSceneName;
        public List<ScenePath> scenePathList;
    }

    [System.Serializable]
    public class ScenePath
    {
        public string sceneName;
        public Vector2Int fromGridCell;
        public Vector2Int gotoGridCell;
    }
}
