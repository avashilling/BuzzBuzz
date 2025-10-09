using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global Game Data")]
    public int flowersPollinated = 0;
    public int totalNectar = 0;

    [Header("UI")]
    public TMP_Text nectarText; // Optional: assign in inspector

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

    /// <summary>
    /// Adds nectar to the total and updates TMP text if assigned
    /// </summary>
    /// <param name="amount">Amount of nectar to add</param>
    public void AddNectarToTotal(int amount)
    {
        totalNectar += amount;
        Debug.Log($"Nectar delivered! Total nectar stored: {totalNectar}");

        // Play the clip already assigned on the AudioSource
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Update TMP text if assigned
        if (nectarText != null)
        {
            nectarText.text = totalNectar.ToString();
        }
    }

    public void ResetGame()
    {
        flowersPollinated = 0;
        totalNectar = 0;

        // Update TMP text if assigned
        if (nectarText != null)
        {
            nectarText.text = "0";
        }

        Debug.Log("Game stats reset for BuzzWorld.");
    }
}
