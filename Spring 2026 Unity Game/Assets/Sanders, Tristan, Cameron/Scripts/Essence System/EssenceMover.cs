using System.Collections.Generic;
using UnityEngine;

public class EssenceMover : MonoBehaviour
{
    [SerializeField] private float force;
    [Header("Audio")]
    [SerializeField] private AudioSource windAudioSource;
    [SerializeField] private AudioClip windBoostClip;
    [SerializeField][Range(0f, 1f)] private float windBoostVolume = 1f;
    [SerializeField][Min(0f)] private float windSoundCooldown = 0.1f;

    [SerializeField] GameObject arrow;
    [SerializeField] GameObject cross;

    private float lastWindSoundTime = float.NegativeInfinity;

    //private Dictionary<Collider2D, float> objectsTimeInWind;

    private void Awake()
    {
        EnsureWindAudioSource();
        //objectsTimeInWind = new();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Essence"))
        {
            PlayWindBoostAudio();
            //objectsTimeInWind.Add(collision, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Essence"))
        {
            ApplyForce(collision);
        }
    }

    private void ApplyForce(Collider2D other)
    {
        if (other.attachedRigidbody == null)
        {
            return;
        }

        Vector2 forceDir = -transform.up;
        other.attachedRigidbody.AddForce(forceDir * force, ForceMode2D.Force);

    }

    private void EnsureWindAudioSource()
    {
        if (windAudioSource == null && windBoostClip != null)
        {
            windAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (windAudioSource == null)
        {
            return;
        }

        windAudioSource.playOnAwake = false;
        windAudioSource.loop = false;
        windAudioSource.spatialBlend = 0f;
    }

    private void PlayWindBoostAudio()
    {
        if (windBoostClip == null)
        {
            return;
        }

        if (Time.time < lastWindSoundTime + windSoundCooldown)
        {
            return;
        }

        EnsureWindAudioSource();
        if (windAudioSource == null)
        {
            return;
        }

        windAudioSource.PlayOneShot(windBoostClip, windBoostVolume);
        lastWindSoundTime = Time.time;
    }

    /// <summary>
    /// Displays an arrow if spell can be placed
    /// Display an X if spell can not be placed
    /// </summary>
    /// <param name="isValid"></param>
    public void DisplayPlaceability(bool isValid)
    {
        if (isValid)
        {
            arrow.SetActive(true);
            cross.SetActive(false);
        }
        else
        {
            arrow.SetActive(false);
            cross.SetActive(true);
        }
    }
}
