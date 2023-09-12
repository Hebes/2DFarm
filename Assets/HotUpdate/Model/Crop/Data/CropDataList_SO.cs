using System.Collections.Generic;
using UnityEngine;

namespace ACFarm
{

    [CreateAssetMenu(fileName = "CropDataList_SO", menuName = "Crop/CropDataList")]
    public class CropDataList_SO : ScriptableObject
    {
        public List<CropDetails> cropDetailsList;
    }
}
