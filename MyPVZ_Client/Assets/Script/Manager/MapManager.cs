using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class MapManager : MonoSingleton<MapManager>
    {
        public Dictionary<Vector2Int,Block> AllBlocks= new Dictionary<Vector2Int,Block>();
        public Dictionary<Vector2Int,Plant> AllPlants= new Dictionary<Vector2Int,Plant>();
        public Dictionary<int, SpawnPoint> AllSpawnPoints = new Dictionary<int, SpawnPoint>();

        Transform blockRoot;
        Transform spawnPointRoot;
        Transform sunSpawnRoot;
        GameObject sunPrefab;

        const int SUN_SPAWN_TIME = 10;
        float sunSpawnTimer;
        protected override void OnAwake()
        {
            base.OnAwake();
            blockRoot = GameObject.Find("Map/Blocks").transform;
            spawnPointRoot = GameObject.Find("Map/SpawnPoints").transform;
            sunSpawnRoot = GameObject.Find("Map/SunSpawnRoot").transform;
            sunPrefab = Resources.Load<GameObject>("Prefab/SunFromSky");
            sunSpawnTimer = UnityEngine.Random.value * SUN_SPAWN_TIME;
        }
        private void Start()
        {
            Init();
        }
        public void Reset()
        {
            sunSpawnTimer = UnityEngine.Random.value * SUN_SPAWN_TIME;
            AllPlants.Clear();
        }
        public void Init()
        {
            Debug.Log("MapManager Init");
            AllBlocks.Clear();
            for (int i = 0; i < blockRoot.childCount; i++)
            {
                Block block = blockRoot.GetChild(i).GetComponent<Block>();
                AllBlocks.Add(block.numPosition, block);
            }

            AllSpawnPoints.Clear();
            for (int i = 0; i < spawnPointRoot.childCount; i++)
            {
                SpawnPoint sp = spawnPointRoot.GetChild(i).GetComponent<SpawnPoint>();
                AllSpawnPoints.Add(sp.index, sp);
            }
            EnemySpawnManager.Instance.rowNum = AllSpawnPoints.Count;
        }
        public void AddPlant(Vector2Int numPostion,Plant plant)
        {
            AllPlants[numPostion] = plant;
        }
        public void RemovePlant(Vector2Int numPostion)
        {
            AllBlocks[numPostion].OnPlantRemove();
            AllPlants.Remove(numPostion);
        }
        public void SpawnZombie(int row, Zombie zombie)
        {
            SpawnPoint spawn = AllSpawnPoints[row];
            spawn.InstantiateEnemy(zombie);
        }
        public void SpawnSun()
        {
            float y = sunSpawnRoot.position.y;
            float x = sunSpawnRoot.position.x + (UnityEngine.Random.value - 0.5f) * 2f * 5f;
            Instantiate(sunPrefab, new Vector3(x, y, 0), Quaternion.identity, sunSpawnRoot);
            Debug.LogFormat("生成阳光:位置[({0}:{1})]", x, y);
        }
        public void OnUpdate()
        {
            sunSpawnTimer -= Time.deltaTime;
            if(sunSpawnTimer <= 0)
            {
                SpawnSun();
                sunSpawnTimer = SUN_SPAWN_TIME;
            }
        }
    }
}

