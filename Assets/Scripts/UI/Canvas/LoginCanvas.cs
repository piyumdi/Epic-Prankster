/*
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

        }(/*)

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

*/




using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yunash.Game;
using Yunash.Audio; // Ensure this is included to access your custom AudioType

namespace Yunash.UI
{
    public class LoginCanvas : CanvasBase
    {
        //
        public static LoginCanvas Instance { get; private set; }
        //

        [Header("Elements")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private AudioSource gameOverAudioSource;
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private AudioSource levelCompleteAudioSource;
        [SerializeField] private GameObject SettingsPanel;
        [SerializeField] private GameObject PausePanel;
        [SerializeField] private GameObject ShopPanel;

        [SerializeField] private GameObject gameTipsPanel;
        [SerializeField] private GameObject ranksPanel;

        [SerializeField] private TMP_Text levelText;

        private AudioManager audioManager; // Declare an AudioManager variable

        //
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        //

        void Start()
        {
            // Initialize the AudioManager
            audioManager = FindObjectOfType<AudioManager>(); // Find the AudioManager in the scene
            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene!");
            }

            // Set initial panel states
            menuPanel.SetActive(true);
            gamePanel.SetActive(false);
            gameOverPanel.SetActive(false);
            levelCompletePanel.SetActive(false);
            SettingsPanel.SetActive(false);
            PausePanel.SetActive(false);
            ShopPanel.SetActive(false);

            gameTipsPanel.SetActive(false); // Hide GameTips panel initially
            ranksPanel.SetActive(false); // Hide Ranks panel initially


            GameManager.onGameStateChanged += GameStateChangedCallBack;
            StartCoroutine(WaitForRoomManager());
        }

        IEnumerator WaitForRoomManager()
        {
            yield return new WaitUntil(() => RoomManager.instance != null); // Wait until RoomManager instance is ready
        }

        private void OnDestroy()
        {
            GameManager.onGameStateChanged -= GameStateChangedCallBack;
            //
            if (Instance == this)
            {
                Instance = null;
            }
            //
        }

        private void GameStateChangedCallBack(GameManager.GameState gameState)
        {
            if (gameState == GameManager.GameState.GameOver)
                ShowGameOver();
            else if (gameState == GameManager.GameState.LevelComplete)
                ShowLevelComplete();
        }

        public void PlayButtonPressed()
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Game);
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);
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

        /*
        private void ShowLevelComplete()
        {
            levelCompletePanel.SetActive(true);
            audioManager?.PlayAudio(Yunash.Audio.AudioType.LevelComplete); // Play level complete music
        }

        public void HideLevelComplete()
        {
            StartCoroutine(HideLevelCompleteCoroutine());
        }

        private IEnumerator HideLevelCompleteCoroutine()
        {
            
            audioManager?.PlayAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);

            yield return new WaitForSeconds(0.5f); 

            levelCompletePanel.SetActive(false);
            LoadNextLevel();
        }


        private void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Ensure the scene index is valid
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No more levels in build settings. Returning to main menu.");
                //SceneManager.LoadScene(0); // Return to main menu or first scene if there are no more levels
                levelCompletePanel.SetActive(false);
            }
        }
        */

        private void ShowLevelComplete()
        {
            levelCompletePanel.SetActive(true);

            audioManager?.StopAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);


            // Play level complete music when the panel is visible
            if (levelCompleteAudioSource != null && !levelCompleteAudioSource.isPlaying)
            {
                levelCompleteAudioSource.Play();
            }

            // Optionally, stop the idle background music if needed
            audioManager?.PlayAudio(Yunash.Audio.AudioType.LevelComplete);
        }

        public void HideLevelComplete()
        {
            StartCoroutine(HideLevelCompleteCoroutine());
        }

        private IEnumerator HideLevelCompleteCoroutine()
        {
            // Optionally, resume idle background music after the level complete panel is hidden
            audioManager?.PlayAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);

            yield return new WaitForSeconds(0.5f); // Adjust timing as necessary

            levelCompletePanel.SetActive(false);
            LoadNextLevel();
        }

        private void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No more levels in build settings. Returning to main menu.");
                levelCompletePanel.SetActive(false);
            }
        }

        /*
        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0; // Freeze the game
        }

        public void RetryButtonPressed()
        {
            Time.timeScale = 1; // Resume normal game speed
            ResetGameState();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
        }
        */
        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0; // Freeze the game

            audioManager?.StopAudio(Yunash.Audio.AudioType.IdleBackgroundMusic);


            // Play game-over music when the gameOverPanel is visible
            if (gameOverAudioSource != null && !gameOverAudioSource.isPlaying)
            {
                gameOverAudioSource.Play();
            }
        }

        public void HideGameOver()
        {
            gameOverPanel.SetActive(false);

            // Stop game-over music when the gameOverPanel is hidden
            if (gameOverAudioSource != null && gameOverAudioSource.isPlaying)
            {
                gameOverAudioSource.Stop();
            }
        }

        public void RetryButtonPressed()
        {
            Time.timeScale = 1; // Resume normal game speed
            ResetGameState();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene

            // Stop the game over music and resume idle background music (or whatever your normal music is)
            if (gameOverAudioSource != null && gameOverAudioSource.isPlaying)
            {
                gameOverAudioSource.Stop();
            }

            // Optionally, restart the normal background music here if needed
            // audioManager?.PlayAudio(Yunash.Audio.AudioType.IdleBackgroundMusic); 
        }


        private void ResetGameState()
        {
            // Reset relevant UI panels
            gamePanel.SetActive(false);
            gameOverPanel.SetActive(false);
            levelCompletePanel.SetActive(false);
            SettingsPanel.SetActive(false);
            PausePanel.SetActive(false);
            ShopPanel.SetActive(false);

            // Reset any other states in GameManager if necessary
            // You might also need to reset persistent data here, e.g., player score, level progress
        }

        // Settings and Shop panel handling methods
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
            Time.timeScale = 0; // Pause the game when showing the pause panel
        }

        public void HidePausePanel()
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1; // Resume the game when hiding the pause panel
        }

        public void ShowShopPanell()
        {
            ShopPanel.SetActive(true);
        }
        public void HideShopPanel()
        {
            ShopPanel.SetActive(false);
        }


        // Add this method if you prefer the ShowGameOverPanel name
        public void ShowGameOverPanel()
        {
            ShowGameOver(); // Reuse the existing method
        }
    }
}
