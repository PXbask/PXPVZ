using Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class LevelManager : MonoSingleton<LevelManager>
    {
        public Action OnLevelSettled; 
        public int Chapter { get => currentLevel.Chapter; }
        public int Level { get => currentLevel.Levels; }
        public int LevelofAll { get => 10 * (Chapter - 1) + Level; }
        public Level currentLevel;
        public GameState state;
        public GameState GameState
        {
            get
            {
                if (isInLevelScene) return state;
                return GameState.None;
            }
            set
            {
                if (value == GameState.None) return;
                if (OnGameStateChanged != null)
                {
                    state= value;
                    OnGameStateChanged.Invoke(value);
                }
            }
        }
        public bool isInLevelScene = false;

        PlayableDirector director;
        public UICardCollection cardCollection;
        public UISunBar sunBar;
        public PlayableDirector MainTimeline { get => director; }
        public List<Plant> selectedPlants;
        public UICardItem selectedCard;

        public Action<UICardItem, bool> OnCardItemSelectAction;
        public Action<GameState> OnGameStateChanged;
        public Action<int> OnSunCollected;

        int sunTotal = 0;

        public int waveProgress = 0;
        public int flagDefend = 0;
        public Vector3 lastPosEnemyDead;

        Trophy trophyC;
        MapLeftTrigger mapLeftTrigger;
        public int SunTotal
        {
            get { return sunTotal; }
            set
            {
                if(OnSunCollected!= null)
                    OnSunCollected.Invoke(value);
                sunTotal = value;
            }
        }
        private void Reset()
        {
            Debug.Log("LevelManager Reset");
            selectedPlants.Clear();
            cardCollection.Clear();
            selectedPlants.Clear();
            SunTotal = 50;
        }
        protected override void OnAwake()
        {
            base.OnAwake();
            director = GameObject.Find("timeline")?.GetComponent<PlayableDirector>();
            cardCollection = GameObject.Find("Canvas-Overlay")?.GetComponentInChildren<UICardCollection>(true);
            sunBar = GameObject.Find("Canvas-Overlay")?.GetComponentInChildren<UISunBar>(true);
            mapLeftTrigger = GameObject.Find("Map/LeftTrigger")?.GetComponent<MapLeftTrigger>();
            mapLeftTrigger.OnZombieEnterHouse += OnLevelFailed;
            OnCardItemSelectAction += OnCardItemSelect;
            OnLevelSettled += ShowSettledAnimation;
        }

        private void Start()
        {
            Init(DataManager.Instance.Levels[UserData.Instance.LevelID]);
        }

        private void SetLevel(Level leveldefine)
        {
            currentLevel = leveldefine;
            state = GameState.SelectCard;
            isInLevelScene = true;
            Debug.LogFormat("<color=#ff0000>½øÈë¹Ø¿¨[{0}-{1}]</color>", leveldefine.Chapter.ToString(), leveldefine.Levels.ToString());
            EnemySpawnManager.Instance.SetLevel(leveldefine);
        }
        public void Init(Level leveldefine)
        {
            Debug.Log("LevelManager Init");
            SetLevel(leveldefine);
            selectedPlants?.Clear();

            cardCollection.Init();
            sunBar.Init();

            selectedPlants = new List<Plant>(10);
            sunTotal = 50;
        }
        private void Update()
        {
            if (!isInLevelScene) return;
            switch (state)
            {
                case GameState.SelectCard:
                    break;
                case GameState.Battle:
                    BattleManager.Instance.OnUpdate();
                    MapManager.Instance.OnUpdate();
                    break;
                case GameState.Settle:
                    break;
                case GameState.Failed:
                    break;
            }
        }

        public void UnPauseMainTimeline()
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
        private void OnCardItemSelect(UICardItem item, bool selected)
        {
            var plant = item.plantDefine;
            if (selected)
            {
                if(selectedPlants.Count < 10) 
                    selectedPlants.Add(plant);
            }
            else
            {
                if (selectedPlants.Count > 1)
                    selectedPlants.Remove(plant);
            }
            cardCollection.Init(true, item, selected);
        }
        private void ShowSettledAnimation()
        {
            GameObject _trophy = Resources.Load<GameObject>("Prefab/trophy");
            var obj = Instantiate(_trophy, lastPosEnemyDead, Quaternion.identity);
            trophyC = obj.GetComponent<Trophy>();
            trophyC.OnClick = ShowSettleAnimation;
        }

        private void ShowSettleAnimation()
        {
            StartCoroutine(SettleAnimation());
        }
        IEnumerator SettleAnimation()
        {
            if (trophyC != null)
            {
                while (trophyC.transform.localScale.x < 2)
                {
                    trophyC.transform.localScale += Vector3.one * 0.5f * Time.deltaTime;
                    yield return null;
                }
            }
            UIReward uIReward = UIManager.Instance.Show<UIReward>();
            uIReward.OnClickNextBtnA = OnClickNextBtnA;
        }
        public void OnClickNextBtnA()
        {
            if (DataManager.Instance.Levels.Count > LevelofAll)
                NextLevel();
            else
                ReturnToMain();
        }

        private void ReturnToMain()
        {
            OnCardItemSelectAction -= OnCardItemSelect;
            OnLevelSettled -= ShowSettledAnimation;
        }

        private void NextLevel()
        {
            UserData.Instance.LevelID = LevelofAll + 1;
            BattleManager.Instance.Reset();
            GameObjectManager.Instance.Reset();
            MapManager.Instance.Reset();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
        public void Restart()
        {
            BattleManager.Instance.Reset();
            GameObjectManager.Instance.Reset();
            MapManager.Instance.Reset();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
        private void OnLevelFailed()
        {
            UIManager.Instance.Show<UIFailed>();
        }
    }
}

