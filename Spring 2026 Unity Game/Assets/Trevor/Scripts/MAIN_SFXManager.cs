using UnityEngine;

public class MAIN_SFXManager : MonoBehaviour
{
    public static MAIN_SFXManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambianceSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Default Background Audio")]
    [SerializeField] private AudioClip defaultMusic;
    [SerializeField] private AudioClip defaultAmbiance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists in the current scene
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        // Start background tracks automatically if assigned
        if (defaultMusic != null) PlayMusic(defaultMusic);
        if (defaultAmbiance != null) PlayAmbiance(defaultAmbiance);
    }

    // Call this from anywhere using MAIN_SFXManager.Instance.PlaySFX(yourClip);
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayAmbiance(AudioClip clip)
    {
        if (clip == null) return;
        ambianceSource.clip = clip;
        ambianceSource.loop = true;
        ambianceSource.Play();
    }
}