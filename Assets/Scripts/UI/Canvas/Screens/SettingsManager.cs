
using UnityEngine;
using UnityEngine.UI;
using Yunash.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button vibrationButton;

    private bool isSoundOn = true;
    private bool isMusicOn = true;
    private bool isVibrationOn = true;

    private AudioManager audioManager;

    void Start()
    {
        // Initialize button listeners
        soundButton.onClick.AddListener(ToggleSound);
        musicButton.onClick.AddListener(ToggleMusic);
        vibrationButton.onClick.AddListener(ToggleVibration);

        // Find and cache reference to the AudioManager in the scene
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in the scene!");
        }
    }

    private void ToggleSound()
    {
        if (audioManager == null) return;

        isSoundOn = !isSoundOn;
        audioManager.MuteSounds(!isSoundOn); // Mute if isSoundOn is false, unmute if true

        Debug.Log("Sound is now " + (isSoundOn ? "On" : "Off"));
    }

    private void ToggleMusic()
    {
        if (audioManager == null) return;

        isMusicOn = !isMusicOn;
        audioManager.MuteMusic(!isMusicOn); // Mute if isMusicOn is false, unmute if true

        Debug.Log("Music is now " + (isMusicOn ? "On" : "Off"));
    }

    private void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        Debug.Log("Vibration is now " + (isVibrationOn ? "On" : "Off"));
        // Add actual vibration control logic if using vibration feedback on supported devices
    }
}
