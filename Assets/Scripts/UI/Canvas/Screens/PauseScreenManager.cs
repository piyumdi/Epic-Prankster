using UnityEngine;
using UnityEngine.UI;

public class PauseScreenManager : MonoBehaviour
{
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;

    void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pause the game
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
    }
}
