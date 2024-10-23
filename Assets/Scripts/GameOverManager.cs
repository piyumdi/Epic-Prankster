using UnityEngine;
using UnityEngine.SceneManagement; // To handle scene reloading or changing scenes

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverScreen; // Reference to the Game Over UI (like a Game Over screen or panel)

    // This method triggers the game over sequence
    public void GameOver()
    {
        // Display the game-over screen if one exists
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        // Optionally, stop the time if you want to freeze the game when it's over
        Time.timeScale = 0f;
    }

    // Restart the game by reloading the current scene
    public void RestartGame()
    {
        Time.timeScale = 1f; // Reset time scale to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload current scene
    }

    // Quit the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
