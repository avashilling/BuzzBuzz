using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip sceneMusic;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            Debug.Log("Audio manager is not null! Playing sceneMusic");
            AudioManager.Instance.PlayMusic(sceneMusic, 1f);
        }
    }
}
