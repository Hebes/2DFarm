﻿using ACFrameworkCore;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	保存数据按钮的组件

-----------------------*/

namespace ACFarm
{
    public class SaveSlotUI : MonoBehaviour
    {
        public Text dataTime, dataScene;
        private Button currentButton;
        private DataSlot currentData;
        private string DataPath;

        private int Index => transform.GetSiblingIndex();

        private void Awake()
        {
            DataPath= Application.persistentDataPath + "/SAVE DATA/" + "data" + Index + ".json";
            dataTime = transform.GetChildComponent<Text>("Time");
            dataScene = transform.GetChildComponent<Text>("Scene");

            currentButton = GetComponent<Button>();
            currentButton.onClick.AddListener(LoadGameData);

            SetupSlotUI();
            transform.GetChildComponent<Button>("Delete").onClick.AddListener(RemoveData);
        }



        private void RemoveData()
        {
            File.Delete(DataPath);
            SaveLoadManagerSystem.Instance.ReadSaveData();
            SetupSlotUI();
        }

       

        private void SetupSlotUI()
        {
            if (true)
            {

            }
            currentData = SaveLoadManagerSystem.Instance.dataSlots[Index];

            if (currentData != null)
            {
                dataTime.text = currentData.DataTime;
                dataScene.text = currentData.DataScene;
            }
            else
            {
                dataTime.text = "这个世界还没开始";
                dataScene.text = "梦还没开始";
            }
        }

        private void LoadGameData()
        {

            if (currentData != null)
            {
                ACDebug.Log($"加载进度{Index}");
                SaveLoadManagerSystem.Instance.Load(Index);
            }
            else
            {
                ACDebug.Log($"新游戏开始");
                ConfigEvent.StartNewGameEvent.EventTrigger(Index);
            }
            ConfigUIPanel.UIMenu.CloseUIPanel();
        }
    }
}
