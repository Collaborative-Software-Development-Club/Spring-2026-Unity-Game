using UnityEngine;

public class JEC_SFXManager : MonoBehaviour
{
    public static JEC_SFXManager Instance;
    [SerializeField] private AudioSource soundFXObject;
    private AudioSource voiceAudioSource;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
        }
    }

    public void PlaySoundEffect(AudioClip audioClip, Transform spawnTransform, float volume, bool randomPitch)
    {
        // spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);
        if (randomPitch)
        {
            audioSource.pitch = UnityEngine.Random.Range(1f, 1.5f);
        } 
        else
        {
            audioSource.pitch = 1f;
        }

            // assign clip
            audioSource.clip = audioClip;

        // assign volume
        audioSource.volume = volume;

        // play sound
        audioSource.Play();

        // get length of SFX clip
        float clipLength = audioSource.clip.length;

        // destroy clip after done playing
        Destroy(audioSource.gameObject, clipLength);

    }

    public void Speak(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        PlaySoundEffect(audioClip, spawnTransform, volume, true);
    }

}
