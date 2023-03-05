using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class InputManager : MonoSingleton<InputManager>
    {
        GameState state;
        Action<GameState> OnGameStateChanged;
        private void Start()
        {
            OnGameStateChanged = (GameState s) => { state = s; };
            LevelManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }
        private void Update()
        {
            switch (state)
            {
                case GameState.Battle:
                    OnBattleInput();
                    break;
            }
        }

        private void OnBattleInput()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (LevelManager.Instance.selectedCard != null)
                {
                    LevelManager.Instance.selectedCard.CancalDisabledState();
                    LevelManager.Instance.selectedCard = null;
                }
            }
        }
    }
}
