using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int flowersPollinated = 0;

    private void Awake() { Instance = this; }

    public void OnFlowerPollinated()
    {
        flowersPollinated++;
        Debug.Log("Amount of flowers pollinated: " + flowersPollinated);
        // Check win condition or update UI
    }
}
