using Yunash.Audio;
using Yunash.Data;
using Yunash.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Yunash.Game.GameManager;

namespace Yunash.Game
{
    public class GameManager : MonoBehaviour
    {
        public enum GameState { Menu, Game, LevelComplete, GameOver }

        [SerializeField] private UIManager uiManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private DataManager dataManager;

        private static GameManager instance;
        public static GameManager Instance => instance;
        public IUIService UIService;
        public IAudioService AudioService;
        public IDataService DataService;

        private GameState gameState;

        public static Action<GameState> onGameStateChanged;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(instance);
                instance = this;
            }
            else
                instance = this;

            if (uiManager == null || audioManager == null || dataManager == null)
            {
                throw new NullReferenceException("GameManager: Initialization: One or More Managers are missing.");
            }

            UIService = uiManager;
            AudioService = audioManager;
            DataService = dataManager;
        }

        void Start()
        {
            //PlayerPrefs.DeleteAll();
        }


        public void SetGameState(GameState gamestate)
        {

            this.gameState = gamestate;
            onGameStateChanged?.Invoke(gamestate);

            Debug.Log("Game State");



        }

        public bool IsGameState()
        {
            return gameState == GameState.Game;
        }

    }
}

