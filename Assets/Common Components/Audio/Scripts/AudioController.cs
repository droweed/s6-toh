using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace gotoandplay
{
    public class AudioController : MonoBehaviour
    {
        public static AudioController Instance;

        public AudioMixer masterMixer;

        public AudioSource bgmSource;
        public AudioSource miscSource;

        public const string PREFS_MAIN_VOLUME_KEY = "_mix_volume_main_key_";
        public const string PREFS_SFX_VOLUME_KEY = "_mix_volume_audio_sfx_key_";


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

        void Start()
        {
            Init();
        }

        void Init()
        {
            var mainVolume = GetMainVolume();
            var sfxVolume = GetSFXVolume();

            SetMainVolume(mainVolume);
            SetSFXVolume(sfxVolume);
        }

        public void SetMainVolume(float value)
        {
            masterMixer.SetFloat("BGMVolume", value);
            PlayerPrefs.SetFloat(PREFS_MAIN_VOLUME_KEY, value);
        }

        public void SetSFXVolume(float value)
        {
            masterMixer.SetFloat("SFXVolume", value);
            PlayerPrefs.SetFloat(PREFS_SFX_VOLUME_KEY, value);
        }

        public float GetMainVolume()
        {
            var value = PlayerPrefs.GetFloat(PREFS_MAIN_VOLUME_KEY, 0);
            return value;
        }

        public float GetSFXVolume()
        {
            var value = PlayerPrefs.GetFloat(PREFS_SFX_VOLUME_KEY, 0);
            return value;
        }

        public void PlayOneShot(AudioClip clip)
        {
            miscSource.clip = clip;
            miscSource.Stop();
            miscSource.Play();
        }

        public void SetBGM(AudioClip clip)
        {
            bgmSource.clip = clip;
            bgmSource.Stop();
            bgmSource.Play();
        }

        public void PlayOneShot(AudioSource audioSource, AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.Stop();
            audioSource.Play();
        }
    }
}