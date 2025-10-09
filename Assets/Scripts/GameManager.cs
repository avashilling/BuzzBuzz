using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global Game Data")]
    public int flowersPollinated = 0;
    public int totalNectar = 0;

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Cache AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "BuzzWorld")
        {
            ResetGame();
        }
    }

    public void OnFlowerPollinated()
    {
        flowersPollinated++;
        Debug.Log($"Flowers pollinated: {flowersPollinated}");
    }

    public void AddNectarToTotal(int amount)
    {
        totalNectar += amount;
        Debug.Log($"Nectar delivered! Total nectar stored: {totalNectar}");

        // Play the clip already assigned on the AudioSource
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void ResetGame()
    {
        flowersPollinated = 0;
        totalNectar = 0;
        Debug.Log("Game stats reset for BuzzWorld.");
    }
}
