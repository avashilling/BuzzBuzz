using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Global Game Data")]
    public int flowersPollinated = 0;
    public int totalNectar = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // persist across scenes
    }

    // Called when the bee pollinates a flower
    public void OnFlowerPollinated()
    {
        flowersPollinated++;
        Debug.Log($"Flowers pollinated: {flowersPollinated}");
        // TODO: check win condition or update UI
    }

    // Called by the bee when depositing nectar at the hive
    public void AddNectarToTotal(int amount)
    {
        totalNectar += amount;
        Debug.Log($"Nectar delivered! Total nectar stored: {totalNectar}");
        // TODO: update global hive UI here
    }

    // Optional: reset the game
    public void ResetGame()
    {
        flowersPollinated = 0;
        totalNectar = 0;
    }
}
