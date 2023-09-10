using ACFrameworkCore;
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

        public List<DataSlot> dataSlots;

        private string jsonFolder;
        private int currentDataIndex;

        public void ICroeInit()
        {
            Instance = this;
            dataSlots = new List<DataSlot>(new DataSlot[3]);
            jsonFolder = Application.persistentDataPath + "/SAVE DATA/";
            ReadSaveData();

            ConfigEvent.StartNewGameEvent.AddEventListener<int>(OnStartNewGameEvent);
            ConfigEvent.EndGameEvent.AddEventListener(OnEndGameEvent);
            MonoManager.Instance.OnAddUpdateEvent(Update);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
                Save(currentDataIndex);
            if (Input.GetKeyDown(KeyCode.O))
                Load(currentDataIndex);
        }

        //事件监听
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

        //数据保存和读取
        public void ReadSaveData()
        {
            for (int i = 0; i < dataSlots.Count; i++)
                dataSlots[i] = null;

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
            foreach (ISaveable saveable in saveableList)
                data.dataDict.Add(saveable.GUID, saveable.GenerateSaveData());
            dataSlots[index] = data;
            string resultPath = jsonFolder + "data" + index + ".json";
            string jsonData = JsonConvert.SerializeObject(dataSlots[index], Formatting.Indented);
            if (!File.Exists(resultPath))
                Directory.CreateDirectory(jsonFolder);
            File.WriteAllText(resultPath, jsonData);
            ACDebug.Log($"数据{index}保存成功");
        }
        public void Load(int index)
        {
            ACDebug.Log($"数据{index}加载成功");
            currentDataIndex = index;
            var resultPath = jsonFolder + "data" + index + ".json";
            var stringData = File.ReadAllText(resultPath);
            var jsonData = JsonConvert.DeserializeObject<DataSlot>(stringData);
            foreach (var saveable in saveableList)
                saveable.RestoreData(jsonData.dataDict[saveable.GUID]);
        }
    }
}
