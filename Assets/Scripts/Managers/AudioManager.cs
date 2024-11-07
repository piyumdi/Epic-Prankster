
using Yunash.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yunash.Audio
{
    public interface IAudioService
    {
        void PlayAudio(AudioType audioType);
        void StopAudio(AudioType audioType);
        void MuteSounds(bool isMute);
        void MuteMusic(bool isMute);
    }

    public class AudioManager : MonoBehaviour, IAudioService
    {
        [SerializeField] private AudioSource soundsAudioSource;  // For sound effects (e.g., button clicks)
        [SerializeField] private AudioSource musicAudioSource;    // For background music
        private AudioData audioData;

        // Track the currently playing music type; initialize to an unused value
        private AudioType currentMusicType = AudioType.MiscAction; // Assume MiscAction won't be used for background music

        private void Start()
        {
            // Get the audio data from GameManager's DataService
            audioData = GameManager.Instance.DataService.AudioData;
            PlayAudio(AudioType.IdleBackgroundMusic); // Start playing background music at the beginning
        }

        public void MuteMusic(bool isMute)
        {
            musicAudioSource.mute = isMute; // Mute/unmute the music
        }

        public void MuteSounds(bool isMute)
        {
            soundsAudioSource.mute = isMute; // Mute/unmute sound effects
        }

        public void PlayAudio(AudioType audioType)
        {
            // Try to get the corresponding audio clip
            audioData.TryGetClip(audioType, out AudioClip clip);

            if (clip == null)
                return; // Exit if no clip is found

            // Check if the audio type is background music
            if (audioType == AudioType.IdleBackgroundMusic || audioType == AudioType.InGameBackgroundMusic || audioType == AudioType.LevelComplete)
            {
                // Play only if the current music type is different
                if (currentMusicType != audioType)
                {
                    musicAudioSource.Stop(); // Stop currently playing music
                    currentMusicType = audioType; // Update the current music type
                    musicAudioSource.clip = clip;   // Set the music clip
                    musicAudioSource.loop = true;   // Loop the background music
                    musicAudioSource.Play();         // Play the music
                }
            }
            else if (audioType == AudioType.ButtonClick)
            {
                soundsAudioSource.clip = clip; // Set the sound clip
                soundsAudioSource.Play();       // Play the sound
            }
        }

        public void StopAudio(AudioType audioType)
        {
            // Stop the appropriate audio based on type
            if (audioType == AudioType.ButtonClick)
            {
                soundsAudioSource.Stop(); // Stop all sound effects
            }
            else if (audioType == AudioType.IdleBackgroundMusic || audioType == AudioType.InGameBackgroundMusic || audioType == AudioType.LevelComplete)
            {
                musicAudioSource.Stop(); // Stop background music
            }
        }
    }

    public enum AudioType
    {
        IdleBackgroundMusic,
        InGameBackgroundMusic,
        LevelComplete,
        EnterGame,
        PanelOpen,
        PanelClose,
        ButtonClick,
        RewardPopup,
        LetterSelect,
        Error,
        Success,
        MiscAction, // This is used to initialize currentMusicType
        ProgressBarFill,
        ProgressBarComplete,
        Shoot
    }
}

