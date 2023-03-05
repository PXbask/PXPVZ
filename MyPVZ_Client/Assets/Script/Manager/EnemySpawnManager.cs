using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using URandom = UnityEngine.Random;

namespace Manager
{
    public class EnemySpawnManager : MonoSingleton<EnemySpawnManager>
    {
        const float RATE_REFRESH_TIME = 5f;
        const float IDLE_TIME = 10f;
        const float FLAG_INSTIST_TIME = 5f;

        GameState state;
        Action<GameState> OnGameStateChanged;
        Level levelData;
        List<Zombie> zombieTypeList = new List<Zombie>();
        List<GameObject> rightZombieObjs = new List<GameObject>();
        Vector3 rightArea;
        Transform zombieRoot;
        public int rowNum;
        bool canSpawnEnemy;

        SpawnState spawnState;
        public SpawnState SpawnState
        {
            get { return spawnState; }
            set
            {
                switch (value)
                {
                    case SpawnState.Idle:
                        break;
                    case SpawnState.Warming:
                        UnPauseSpawning();
                        break;
                    case SpawnState.Warmed:
                        levelData.MaxCount /= 2;
                        if(spawnMaxCount>=levelData.MaxCount)
                            spawnMaxCount= levelData.MaxCount;
                        break;
                    case SpawnState.WaveWarming:
                        PauseSpawning();
                        break;
                    case SpawnState.Wave:
                        FlagDefend++;
                        Invoke(nameof(ChangeSpawnStateToOther), FLAG_INSTIST_TIME);
                        UnPauseSpawning();
                        levelData.MaxCount *= 2;
                        spawnMaxCount= levelData.MaxCount;
                        break;
                    case SpawnState.End:
                        spawnIntensity = 0;
                        spawnDegree = 0;
                        spawnMaxCount= 0;
                        break;
                }
                spawnState = value;
                Debug.LogFormat("<color=#ff00ff>当前SpawnState: {0}</color>",value.ToString());
            }
        }
        [SerializeField] float spawnIntensity;
        [SerializeField] float spawnDegree;
        [SerializeField] float spawnMaxCount;
        Zombie nextZombie;
        Dictionary<Zombie,float> SpawnRates = new Dictionary<Zombie,float>();
        [SerializeField] int EnemyCount => GameObjectManager.Instance.GetAllEnemyCounts();
        [SerializeField] int WaveProgress
        {
            get => LevelManager.Instance.waveProgress;
            set => LevelManager.Instance.waveProgress = value;
        }
        [SerializeField] int FlagDefend
        {
            get => LevelManager.Instance.flagDefend;
            set => LevelManager.Instance.flagDefend = value;
        }
        private void Awake()
        {
            GameObject area = GameObject.Find("Map/RightArea");
            zombieRoot = GameObject.Find("root/ZombieRoot")?.transform;
            rightArea = area.transform.position;
        }
        private void Start()
        {
            Init();
        }
        public void Init()
        {
            OnGameStateChanged = (GameState s) =>
            {
                state = s;
                if (s == GameState.Battle)
                {
                    Invoke(nameof(ChangeSpawnStateToWarming), IDLE_TIME);
                    for(int i=0;i<rightZombieObjs.Count;i++)
                    {
                        Destroy(rightZombieObjs[i]);
                    }
                }

            };
            LevelManager.Instance.OnGameStateChanged += OnGameStateChanged;
            canSpawnEnemy = false;
            nextZombie = zombieTypeList[0];
            SpawnState = SpawnState.Idle;
        }
        public void Reset()
        {
            canSpawnEnemy = false;
            nextZombie = zombieTypeList[0];
            SpawnState = SpawnState.Idle;
            SpawnRates.Clear();

            spawnDegree= 0;
            spawnIntensity= 0;
            spawnMaxCount= 0;
        }
        private void Update()
        {
            HandleSpawnParas();
            switch (state)
            {
                case GameState.Battle:
                    OnBattleUpdate();
                    break;
            }
        }

        private void HandleSpawnParas()
        {
            if (state != GameState.Battle) return;
            switch (SpawnState)
            {
                case SpawnState.Idle:
                    spawnIntensity= 0;
                    SpawnState = SpawnState.Warming;
                    break;
                case SpawnState.Warming:
                    spawnIntensity += levelData.Urgentment * Time.deltaTime;
                    if (spawnIntensity > levelData.Difficulty) SpawnState = SpawnState.Warmed;
                    break;
                case SpawnState.Warmed:
                    break;
                case SpawnState.WaveWarming:
                    break;
                case SpawnState.Wave:
                    break;
                case SpawnState.End:
                    if (spawnIntensity <= 0 && spawnDegree <= 0 && EnemyCount <= 0)
                    {
                        state = GameState.Settle;
                        LevelManager.Instance.OnLevelSettled.Invoke();
                        spawnState = SpawnState.None;
                    }
                    break;
                case SpawnState.None:
                    break;
            }
            spawnDegree += spawnIntensity * Time.deltaTime;

            if(spawnMaxCount<levelData.MaxCount)
                spawnMaxCount += levelData.Urgentment *Time.deltaTime *levelData.CountRatio;
        }
        private void PauseSpawning()
        {
            canSpawnEnemy = false;
        }
        private void UnPauseSpawning()
        {
            canSpawnEnemy = true;
        }

        public void SetLevel(Level level)
        {
            levelData = level;
            zombieTypeList.Clear();
            foreach (var item in levelData.ZombieTypes)
            {
                Zombie zombie = DataManager.Instance.Zombies[item];
                zombieTypeList.Add(zombie);
            }
            InitRightAreaZombies();
        }
        private void InitRightAreaZombies()
        {
            foreach (var zombie in zombieTypeList)
            {
                Zombie baseZombie;
                if (zombie.Base != 0)
                    baseZombie = DataManager.Instance.Zombies[zombie.Base];
                else baseZombie = zombie;

                GameObject obj = Resources.Load<GameObject>(baseZombie.PrefabRes);
                Vector3 v3 = URandom.insideUnitCircle * 2;
                GameObject zobj = Instantiate(obj, rightArea + v3, Quaternion.identity, zombieRoot);
                ZombieLogic zombieLogic = zobj.GetComponent<ZombieLogic>();
                zombieLogic.Init(zombie, null, SpawnState, false);
                rightZombieObjs.Add(zobj);
                Debug.LogFormat("生成[{0}]---地点：地图右侧", zombie.Name);
            }
        }

        private void OnBattleUpdate()
        {
            RefreshRateUpdate();
            SpawnZombies();
            CheckWave();
        }

        private void CheckWave()
        {
            if (FlagDefend < levelData.FlagProgress.Count)
            {
                if (WaveProgress >= levelData.FlagProgress[FlagDefend] * 0.8f)
                {
                    SpawnState = SpawnState.WaveWarming;
                }
                if (WaveProgress >= levelData.FlagProgress[FlagDefend])
                {
                    SpawnState = SpawnState.Wave;
                }
            }
        }

        float RateRefreshTimer;
        private void RefreshRateUpdate()
        {
            RateRefreshTimer -= Time.deltaTime;
            if (RateRefreshTimer <= 0)
            {
                RefreshRateList();
                RateRefreshTimer = RATE_REFRESH_TIME;
            }
        }
        private void RefreshRateList()
        {
            SpawnRates.Clear();
            foreach (var item in zombieTypeList)
            {
                if(item.Menace>spawnIntensity)
                {
                    SpawnRates.Add(item, 0);
                }
                else
                {
                    float rate = CalculateRate(item.Menace);
                    SpawnRates.Add(item, rate);
                }
            }
            DebugRateList();
        }

        private void DebugRateList()
        {
            StringBuilder sb= new StringBuilder();
            sb.AppendLine("概率列表已更新");
            float sum = SpawnRates.Values.Sum();
            foreach (var item in SpawnRates)
            {
                sb.AppendLine(string.Format("名称【{0}】 生成概率:[{1}]", item.Key.Name, item.Value / sum));
            }
            Debug.Log(sb.ToString());
        }

        public float CalculateRate(int menace)
        {
            //刷怪公式 Rate = -|Menace-(spawnSpeed/2f)|*a+0.5f a=0.6f/spawnSpeed 每5秒刷新一次Rate集合
            float diff = spawnIntensity;
            float tmp = MathF.Abs(menace - (diff / 2f));
            return -tmp * (0.6f - diff) + 0.5f;
        }
        private void SpawnZombies()
        {
            if (spawnMaxCount <= EnemyCount) return;
            if (!canSpawnEnemy) return;
            if (nextZombie != null)
            {
                if (nextZombie.Menace <= spawnDegree)
                {
                    SpawnZombie(nextZombie);
                    nextZombie = CalculateNextZombie();
                }
            }
            else
            {
                nextZombie = CalculateNextZombie();
            }
        }

        private Zombie CalculateNextZombie()
        {
            float rate = URandom.Range(0, SpawnRates.Values.Sum());
            float currProb = 0;
            foreach (var item in SpawnRates)
            {
                currProb += item.Value;
                if (rate <= currProb)
                {
                    Debug.LogFormat("下一个僵尸：【{0}】", item.Key.Name);
                    return item.Key;
                }
            }
            return null;
        }

        private void SpawnZombie(Zombie zombie)
        {
            int row = URandom.Range(1, rowNum + 1);
            MapManager.Instance.SpawnZombie(row, zombie);
            spawnDegree -= zombie.Menace;
        }

        public void InstantiateEnemy(Zombie zombie, SpawnPoint spawnPoint)
        {
            Zombie baseZombie;
            if (zombie.Base != 0)
                baseZombie = DataManager.Instance.Zombies[zombie.Base];
            else baseZombie = zombie;

            GameObject obj = Resources.Load<GameObject>(baseZombie.PrefabRes);
            GameObject zobj = Instantiate(obj, spawnPoint.Position, Quaternion.identity, zombieRoot);
            ZombieLogic zombieLogic = zobj.GetComponent<ZombieLogic>();
            zombieLogic.Init(zombie, spawnPoint, SpawnState, true);

            GameObjectManager.Instance.AddEnemy(spawnPoint.index, zobj);
            Debug.LogFormat("生成[{0}]---地点：第{1}行", zombie.Name, spawnPoint.index);
        }
        void ChangeSpawnStateToWarming()
        {
            SpawnState = SpawnState.Warming;
        }
        void ChangeSpawnStateToOther()
        {
            WaveProgress = 0;
            if(FlagDefend>=levelData.FlagProgress.Count)
            {
                SpawnState = SpawnState.End;
            }
            else
            {
                SpawnState = SpawnState.Warmed;
            }
        }
    }
}
