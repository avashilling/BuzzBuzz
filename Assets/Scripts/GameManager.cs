using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int flowersPollinated = 0;
    public int totalNectar = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void OnFlowerPollinated()
    {
        flowersPollinated++;
        // Check win condition or update UI
    }

    // called by the bee when depositing nectar at the hive
    public void AddNectarToTotal(int amount)
    {
        totalNectar += amount;
        Debug.Log($"Nectar delivered! Total nectar stored: {totalNectar}");
        // TODO: update global hive UI here
    }
}
