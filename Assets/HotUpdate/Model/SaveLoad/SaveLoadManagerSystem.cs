﻿using ACFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/*--------脚本描述-----------

电子邮箱：
	1607388033@qq.com
作者:
	暗沉
描述:
	记录保存管理系统

-----------------------*/

namespace ACFarm
{
    public class SaveLoadManagerSystem : ICore
    {
        public static SaveLoadManagerSystem Instance;
        

        private List<ISaveable> saveableList = new List<ISaveable>();

        public List<DataSlot> dataSlots = new List<DataSlot>(new DataSlot[3]);

        private string jsonFolder;
        private int currentDataIndex;

        public void ICroeInit()
        {
            Instance = this;
            jsonFolder = Application.persistentDataPath + "/SAVE DATA/";
            ReadSaveData();

            ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
            ConfigEvent.EndGameEvent.AddEventListener(OnEndGameEvent);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                Save(currentDataIndex);
            if (Input.GetKeyDown(KeyCode.O))
                Load(currentDataIndex);
        }


        private void OnEndGameEvent()
        {
            Save(currentDataIndex);
        }
        private void OnStartNewGameEvent(int index)
        {
            currentDataIndex = index;
        }
        public void RegisterSaveable(ISaveable saveable)
        {
            if (!saveableList.Contains(saveable))
                saveableList.Add(saveable);
        }

        private void ReadSaveData()
        {
            if (Directory.Exists(jsonFolder))
            {
                for (int i = 0; i < dataSlots.Count; i++)
                {
                    var resultPath = jsonFolder + "data" + i + ".json";
                    if (File.Exists(resultPath))
                    {
                        var stringData = File.ReadAllText(resultPath);
                        var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);
                        dataSlots[i] = jsonData;
                    }
                }
            }
        }


        private void Save(int index)
        {
            DataSlot data = new DataSlot();

            foreach (var saveable in saveableList)
            {
                data.dataDict.Add(saveable.GUID, saveable.GenerateSaveData());
            }
            dataSlots[index] = data;

            var resultPath = jsonFolder + "data" + index + ".json";

            var jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);

            if (!File.Exists(resultPath))
            {
                Directory.CreateDirectory(jsonFolder);
            }
            Debug.Log("DATA" + index + "SAVED!");
            File.WriteAllText(resultPath, jsonData);
        }

        public void Load(int index)
        {
            //currentDataIndex = index;

            //var resultPath = jsonFolder + "data" + index + ".json";

            //var stringData = File.ReadAllText(resultPath);

            //var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);

            //foreach (var saveable in saveableList)
            //{
            //    saveable.RestoreData(jsonData.dataDict[saveable.GUID]);
            //}
        }
    }
}
