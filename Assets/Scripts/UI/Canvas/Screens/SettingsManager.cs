using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button vibrationButton;

    private bool isSoundOn = true;
    private bool isMusicOn = true;
    private bool isVibrationOn = true;

    void Start()
    {
        // Initialize button listeners
        soundButton.onClick.AddListener(ToggleSound);
        musicButton.onClick.AddListener(ToggleMusic);
        vibrationButton.onClick.AddListener(ToggleVibration);
    }

    private void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        
        Debug.Log("Sound is now " + (isSoundOn ? "On" : "Off"));
        // Implement logic to enable/disable sound in the future when sounds are added
    }

    private void ToggleMusic()
    {
        isMusicOn = !isMusicOn;
        Debug.Log("Music is now " + (isMusicOn ? "On" : "Off"));
        // Implement logic to enable/disable music in the future when music is added
    }

    private void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        Debug.Log("Vibration is now " + (isVibrationOn ? "On" : "Off"));
        // Implement logic to enable/disable vibration in the future
    }
}
