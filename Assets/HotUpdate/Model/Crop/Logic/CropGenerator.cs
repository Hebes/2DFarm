using Core;
using UnityEngine;

/*--------�ű�����-----------

�������䣺
    1607388033@qq.com
����:
    ����
����:
    ׯ������

-----------------------*/


namespace Farm2D
{
    public class CropGenerator : MonoBehaviour
    {
        private Grid currentGrid;

        public int seedItemID;
        public int growthDays;

        private void Awake()
        {
            currentGrid = FindObjectOfType<Grid>();
        }
        private void OnEnable()
        {
            ConfigEvent.GenerateCrop.AddEventListener(GenerateCrop);
        }
        private void OnDisable()
        {
            ConfigEvent.GenerateCrop.RemoveEventListener(GenerateCrop);
        }

        private void GenerateCrop()
        {
            Vector3Int cropGridPos = currentGrid.WorldToCell(transform.position);

            if (seedItemID != 0)
            {
                var tile = GridMapManagerSystem.Instance.GetTileDetailsOnMousePosition(cropGridPos);

                if (tile == null)
                {
                    tile = new TileDetails();
                    tile.girdX = cropGridPos.x;
                    tile.gridY = cropGridPos.y;
                }

                tile.daysSinceWatered = -1;
                tile.seedItemID = seedItemID;
                tile.growthDays = growthDays;

                GridMapManagerSystem.Instance.UpdateTileDetails(tile);
            }
        }
    }
}