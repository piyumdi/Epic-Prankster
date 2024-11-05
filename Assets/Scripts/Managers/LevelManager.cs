using UnityEngine;
using UnityEngine.UI;
//
using Yunash.UI;  // Add this line
//

public class LevelManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public GameObject levelCompleteUI;
    public Button nextLevelButton;

    public int maxEnemyPower = 10;
    private int currentEnemyPower;
    private bool levelComplete;

    void Start()
    {
        nextLevelButton.onClick.AddListener(LoadNextLevel);
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        levelComplete = false;
        currentEnemyPower = maxEnemyPower;  // Reset enemy power
        levelCompleteUI.SetActive(false);    // Hide level complete UI

        // Set initial positions for player and enemy in the new room
        ResetPlayerAndEnemyPosition();

        // Ensure enemy and player start in default states
        player.SetActive(true);
        enemy.SetActive(true);
    }

    private void ResetPlayerAndEnemyPosition()
    {
        // Assuming you have initial positions set for each level
        player.transform.position = new Vector3(0, 1, 0);  // Adjust position as needed
        enemy.transform.position = new Vector3(5, 1, 0);   // Adjust position as needed
    }

    public void OnPlayerShoot()
    {
        if (levelComplete) return;

        currentEnemyPower--;

        if (currentEnemyPower <= 0)
        {
            ShowLevelComplete();
        }
    }

    private void ShowLevelComplete()
    {
        levelComplete = true;
        levelCompleteUI.SetActive(true);
        player.SetActive(false);  // Hide player to avoid interaction after level completion
    }

    public void LoadNextLevel()
    {
        //
        LoginCanvas.Instance.HideLevelComplete();
        //
        InitializeLevel();
    }
}

