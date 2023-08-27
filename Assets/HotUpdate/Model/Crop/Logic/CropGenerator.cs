using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ACFrameworkCore
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
                var tile = GridMapSystem.Instance.GetTileDetailsOnMousePosition(cropGridPos);

                if (tile == null)
                {
                    tile = new TileDetails();
                    tile.girdX = cropGridPos.x;
                    tile.gridY = cropGridPos.y;
                }

                tile.daysSinceWatered = -1;
                tile.seedItemID = seedItemID;
                tile.growthDays = growthDays;

                GridMapSystem.Instance.UpdateTileDetails(tile);
            }
        }
    }
}