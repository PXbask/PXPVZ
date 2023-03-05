using Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class LevelSceneReceiver : MonoBehaviour
{
    PlayableDirector Director { get => LevelManager.Instance.MainTimeline; }
    public void StartSelectCards()
    {
        Director.playableGraph.GetRootPlayable(0).SetSpeed(0);
        UIManager.Instance.Show<UISelectPlant>();
        LevelManager.Instance.cardCollection.gameObject.SetActive(true);
    }
    public void StartBattle()
    {
        LevelManager.Instance.GameState = GameState.Battle;
        BattleManager.Instance.Init();
    }
}
