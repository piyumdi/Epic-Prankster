
using UnityEngine;
using UnityEngine.UI;
using Yunash.Audio; // Make sure this namespace matches your AudioManager’s namespace

public class PauseScreenManager : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    private AudioManager audioManager;

    void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);

        // Find and cache the AudioManager instance
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
        if (audioManager != null)
        {
            audioManager.MuteMusic(true); // Pause the background music
        }
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        if (audioManager != null)
        {
            audioManager.MuteMusic(false); // Resume the background music
        }
    }
}
