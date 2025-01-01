using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        public AudioMixerGroup mixerGroup;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(0.1f, 3f)] public float pitch = 1f;
        public bool loop;
        
        [HideInInspector] public AudioSource source;
    }

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Sound[] sounds;
    
    private Dictionary<string, Sound> soundDictionary = new Dictionary<string, Sound>();

    [Header("Dimension Music")]
    [SerializeField] private AudioClip[] dimensionMusic;
    private AudioSource musicSource;
    private int currentMusicIndex = -1;

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
        foreach (Sound s in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            s.source = source;

            source.clip = s.clip;
            source.outputAudioMixerGroup = s.mixerGroup;
            source.volume = s.volume;
            source.pitch = s.pitch;
            source.loop = s.loop;

            soundDictionary[s.name] = s;
        }
    }

    public void PlaySound(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            sound.source.Play();
        }
        else
        {
            Debug.LogWarning($"Sound: {name} not found!");
        }
    }

    public void StopSound(string name)
    {
        if (soundDictionary.TryGetValue(name, out Sound sound))
        {
            sound.source.Stop();
        }
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    private void Start()
    {
        // Müzik için ayrı bir AudioSource oluştur
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.outputAudioMixerGroup = soundDictionary["Music"].mixerGroup;
        musicSource.loop = true;
    }

    public void PlayDimensionMusic(int dimensionId)
    {
        if (dimensionId < 0 || dimensionId >= dimensionMusic.Length) return;
        if (currentMusicIndex == dimensionId) return;

        currentMusicIndex = dimensionId;
        if (musicSource != null)
        {
            musicSource.clip = dimensionMusic[dimensionId];
            musicSource.Play();
        }
    }
} 