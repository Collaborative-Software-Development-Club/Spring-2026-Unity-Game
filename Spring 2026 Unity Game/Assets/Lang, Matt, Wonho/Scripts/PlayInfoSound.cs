using UnityEngine;

public class PlayInfoSound : MonoBehaviour
{
    public AudioSource buttonAudioSource; //Reference to your Audio Source
    public AudioSource checkAudioSource;

    // A public function that can be called by the button's OnClick event
    public void PlayButtonSound()
    {
        if (buttonAudioSource != null)
        {
            buttonAudioSource.Stop();
            buttonAudioSource.Play();
        }
    }

    public void PlayCheckSound()
    {
        if (checkAudioSource != null)
        {
            checkAudioSource.Stop();
            checkAudioSource.Play();
        }
    } 
}
