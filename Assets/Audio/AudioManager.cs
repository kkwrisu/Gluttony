using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("UI SFX")]
    public AudioClip buttonClickClip;

    [Header("Gameplay SFX")]
    public AudioClip itemPickupClip;
    public AudioClip itemDropClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume;

        if (sfxSource != null)
            sfxSource.volume = sfxVolume;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneMusic sceneMusic = FindFirstObjectByType<SceneMusic>();
        if (sceneMusic != null)
        {
            PlayMusic(sceneMusic.musicClip);
        }
    }


    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource == null) return;

        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }


    public void PlaySFX(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip);
    }


    public void PlayButtonClick()
    {
        PlaySFX(buttonClickClip);
    }

    public void PlayItemPickup()
    {
        PlaySFX(itemPickupClip);
    }

    public void PlayItemDrop()
    {
        PlaySFX(itemDropClip);
    }


    public void SetMusicVolume(float value)
    {
        musicVolume = value;

        if (musicSource != null)
            musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;

        if (sfxSource != null)
            sfxSource.volume = value;
    }
}