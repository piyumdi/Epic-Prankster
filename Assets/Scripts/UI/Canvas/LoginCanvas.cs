using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        
        [SerializeField] private GameObject gameTipsPanel;
        [SerializeField] private GameObject ranksPanel;
        

        [SerializeField] private TMP_Text levelText;



        void Start()
        {
            menuPanel.SetActive(true);
            gamePanel.SetActive(false);
            gameOverPanel.SetActive(false);
            SettingsPanel.SetActive(false);
            PausePanel.SetActive(false);
            ShopPanel.SetActive(false);
            levelCompletePanel.SetActive(false);
            
            gameTipsPanel.SetActive(false); // Hide GameTips panel initially
            ranksPanel.SetActive(false); // Hide Ranks panel initially
            

            GameManager.onGameStateChanged += GameStateChangedCallBack;
                StartCoroutine(WaitForRoomManager());

            
        }
        IEnumerator WaitForRoomManager()
        {
            yield return new WaitUntil(() => RoomManager.instance != null); // Wait until RoomManager instance is ready

            //levelText.text = "Level " + (RoomManager.instance.GetLevel() + 1);
        }


        
        public void ShowGameTipsPanel()
        {
            gameTipsPanel.SetActive(true);
        }

        public void HideGameTipsPanel()
        {
            gameTipsPanel.SetActive(false);
        }

        public void ShowRanksPanel()
        {
            ranksPanel.SetActive(true);
        }

        public void HideRanksPanel()
        {
            ranksPanel.SetActive(false);
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
            Time.timeScale = 1; // Resume normal game speed
            SceneManager.LoadScene(0); // Reload the scene
        }

        /*
        public void ShowGameOver()
        {
            //gamePanel.SetActive(false);
            gameOverPanel.SetActive(true);

        }*/

        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0; // Freeze the game
        }


        private void ShowLevelComplete()
        {
            //gamePanel.SetActive(false);
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

        public void HideLevelComplete()
        {

            Debug.Log("Clicked");
            gamePanel.SetActive(true);
            levelCompletePanel.SetActive(false);

            Debug.Log("end");
            
        }

        //
        public void ShowGameOverPanel()
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(true);
                //Time.timeScale = 0; // Freeze the game
            }
        }
        //
    }

}

