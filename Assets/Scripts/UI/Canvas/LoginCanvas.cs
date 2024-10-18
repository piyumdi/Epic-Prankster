using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Yunash.Game;

namespace Yunash.UI
{
    public class LoginCanvas : CanvasBase
    {

        [Header("Elements")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private GameObject SettingsPanel;
        [SerializeField] private GameObject PausePanel;
        [SerializeField] private GameObject ShopPanel;


        void Start()
        {
            menuPanel.SetActive(true);
            gamePanel.SetActive(false);
            gameOverPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            PausePanel.SetActive(false);
            ShopPanel.SetActive(false);

            GameManager.onGameStateChanged += GameStateChangedCallBack;
        }

        private void OnDestroy()
        {
            GameManager.onGameStateChanged -= GameStateChangedCallBack;

        }

        private void GameStateChangedCallBack(GameManager.GameState gameState)
        {
            if (gameState == GameManager.GameState.GameOver)

                ShowGameOver();

            else if (gameState == GameManager.GameState.LevelComplete)
                ShowLevelComplete();

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void PlayButtonPressed()
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Game);
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);

        }

        public void RetryButtonPressed()
        {
            SceneManager.LoadScene(0);
        }

        public void ShowGameOver()
        {
            gamePanel.SetActive(false);
            gameOverPanel.SetActive(true);

        }

        private void ShowLevelComplete()
        {
            gamePanel.SetActive(false);
            levelCompletePanel.SetActive(true);
        }

        
        public void ShowSettingsPanel()
        {
            SettingsPanel.SetActive(true);
        }
        public void HideSettingsPanel()
        {
            SettingsPanel.SetActive(false);
        }
        
        public void ShowPausePanel()
        {
            PausePanel.SetActive(true);
        }
        public void HidePausePanel()
        {
            PausePanel.SetActive(false);
        }
        
        public void ShowShopPanell()
        {
            ShopPanel.SetActive(true);
        }
        public void HideShopPanel()
        {
            ShopPanel.SetActive(false);
        }
    }

}

