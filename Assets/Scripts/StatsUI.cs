using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    public TMP_Text pollenText;  // drag your pollen TMP object here
    public TMP_Text nectarText;  // drag your nectar TMP object here

    void Start()
    {
        // Make sure GameManager exists
        if (GameManager.Instance != null)
        {
            // Set the text once from the GameManager's saved stats
            if (pollenText != null)
                pollenText.text = GameManager.Instance.flowersPollinated.ToString();

            if (nectarText != null)
                nectarText.text = GameManager.Instance.totalNectar.ToString();
        }
    }
}
