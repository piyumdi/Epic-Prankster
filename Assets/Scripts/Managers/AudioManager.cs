/*
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
        [SerializeField] AudioSource soundsAudioSource;
        [SerializeField] AudioSource musicAudioSource;
        private AudioData audioData;

        private void Start()
        {
            audioData = GameManager.Instance.DataService.AudioData;
        }
        public void MuteMusic(bool isMute)
        {
            musicAudioSource.mute = isMute;
        }

        public void MuteSounds(bool isMute)
        {
            soundsAudioSource.mute = isMute;
        }

        public void PlayAudio(AudioType audioType)
        {
            audioData.TryGetClip(audioType, out AudioClip clip);

            if (clip == null)
                return;

            if (audioType == AudioType.ButtonClick)
            {
                soundsAudioSource.clip = clip;
                soundsAudioSource.Play();
            }
            else if (audioType == AudioType.IdleBackgroundMusic)
            {
                musicAudioSource.clip = clip;
                musicAudioSource.Play();
            }
        }

        public void StopAudio(AudioType audioType)
        {
        }
    }

    public enum AudioType
    {
        IdleBackgroundMusic,
        InGameBackgroundMusic,
        EnterGame,
        PanelOpen,
        PanelClose,
        ButtonClick,
        RewardPopup,
        LetterSelect,
        Error,
        Success,
        MiscAction,
        LevelComplete,
        ProgressBarFill,
        ProgressBarComplete
    }
}
*/

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

        // Track the currently playing music type
        private AudioType currentMusicType;

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
                // Stop the current music if it's not the same as the new one
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
        MiscAction,
        ProgressBarFill,
        ProgressBarComplete
    }
}

