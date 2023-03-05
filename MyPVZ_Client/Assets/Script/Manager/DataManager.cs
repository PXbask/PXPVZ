using Model;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Manager
{
    public class DataManager : Singleton<DataManager>
    {
        string dataPath;
        public Dictionary<int, Zombie> Zombies = new Dictionary<int, Zombie>();
        public Dictionary<int, Equipment> Equipments = new Dictionary<int, Equipment>();
        public Dictionary<int, Level> Levels = new Dictionary<int, Level>();
        public Dictionary<int, Plant> Plants= new Dictionary<int, Plant>();
        public Dictionary<int, Bullet> Bullets = new Dictionary<int, Bullet>();
        public DataManager()
        {
            dataPath = "Data/";
            Debug.Log("DataManager Init");
        }
        public IEnumerator LoadData()
        {
            string json = File.ReadAllText(dataPath + "ZombieDefine.txt");
            this.Zombies = JsonConvert.DeserializeObject<Dictionary<int, Zombie>>(json);
            yield return null;

            json = File.ReadAllText(dataPath + "EquipmentDefine.txt");
            this.Equipments = JsonConvert.DeserializeObject<Dictionary<int, Equipment>>(json);
            yield return null;

            json = File.ReadAllText(dataPath + "LevelDefine.txt");
            this.Levels = JsonConvert.DeserializeObject<Dictionary<int, Level>>(json);
            yield return null;

            json = File.ReadAllText(dataPath + "PlantDefine.txt");
            this.Plants = JsonConvert.DeserializeObject<Dictionary<int, Plant>>(json);
            yield return null;

            json = File.ReadAllText(dataPath + "BulletDefine.txt");
            this.Bullets = JsonConvert.DeserializeObject<Dictionary<int, Bullet>>(json);
            yield return null;
        }
    }
}


