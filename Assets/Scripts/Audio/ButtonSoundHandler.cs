using UnityEngine;
using UnityEngine.UI;
using Yunash.Audio;

public class ButtonSoundHandler : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        // Find the AudioManager instance in the scene
        audioManager = FindObjectOfType<AudioManager>();

        // Add the button click listener to each button this script is attached to
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayButtonClickSound);
        }
    }

    private void PlayButtonClickSound()
    {
        // Play the button click sound
        if (audioManager != null)
        {
            audioManager.PlayAudio(Yunash.Audio.AudioType.ButtonClick); // Explicit namespace to avoid ambiguity
        }
    }
}
