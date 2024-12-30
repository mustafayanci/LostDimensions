using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class SoundEffect
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;
        [HideInInspector] public AudioSource source;
    }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    
    [Header("Audio Mixers")]
    [SerializeField] private AudioMixerGroup musicMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;
    
    [Header("Sound Effects")]
    [SerializeField] private SoundEffect[] soundEffects;
    
    [Header("Background Music")]
    [SerializeField] private AudioClip[] dimensionMusic;
    
    private Dictionary<string, SoundEffect> soundDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio()
    {
        soundDictionary = new Dictionary<string, SoundEffect>();
        
        foreach (var sound in soundEffects)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sfxMixer;
            
            soundDictionary.Add(sound.name, sound);
        }
    }

    public void PlaySound(string name)
    {
        if (soundDictionary.TryGetValue(name, out SoundEffect sound))
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound {name} not found!");
        }
    }

    public void PlayDimensionMusic(int dimensionId)
    {
        if (dimensionId >= 0 && dimensionId < dimensionMusic.Length)
        {
            StartCoroutine(CrossfadeMusic(dimensionMusic[dimensionId]));
        }
    }

    private System.Collections.IEnumerator CrossfadeMusic(AudioClip newMusic)
    {
        float fadeTime = 1f;
        float elapsedTime = 0f;
        
        // Mevcut müziği kıs
        while (elapsedTime < fadeTime)
        {
            musicSource.volume = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        // Yeni müziği ayarla
        musicSource.clip = newMusic;
        musicSource.Play();
        elapsedTime = 0f;
        
        // Yeni müziği yükselt
        while (elapsedTime < fadeTime)
        {
            musicSource.volume = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
} 