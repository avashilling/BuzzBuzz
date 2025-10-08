using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource musicSource;

    private AudioClip currentClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayMusic(AudioClip newClip, float fadeDuration = 1f)
    {
        // if it's already playing this clip, skip fading entirely
        if (musicSource.clip == newClip && musicSource.isPlaying)
            return;

        StartCoroutine(FadeToNewMusic(newClip, fadeDuration));
    }

    private IEnumerator FadeToNewMusic(AudioClip newClip, float fadeDuration)
    {
        Debug.Log("FadeToNewMusic called! newClip: " + newClip.name);
        if (musicSource.isPlaying && musicSource.clip != newClip)
        {
            float startVolume = musicSource.volume;

            // fade out
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }

            musicSource.Stop();
            musicSource.volume = startVolume;
        }

        if (newClip != null)
        {
            // fade in
            musicSource.clip = newClip;
            musicSource.Play();

            musicSource.volume = 0;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(0, 1, t / fadeDuration);
                yield return null;
            }

            musicSource.volume = 1;
        }

        currentClip = newClip;
    }
}
