using UnityEngine;
using UnityEngine.UI;

public class TileClicked : MonoBehaviour
{
    public int row;
    public int column;
    public PlayerControll control; // reference to main manager

    public void OnClick()
    {
        if (control != null)
        {
            control.TileClicked(row, column);
            PlayButtonSound();
        }
        else
        {
            Debug.LogWarning("control not assigned!");
        }
    }

    public AudioSource audioSource; //Reference to your Audio Source

    // A public function that can be called by the button's OnClick event
    public void PlayButtonSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            audioSource.time = 4.5f;
            audioSource.Play();
        }
    }
}
