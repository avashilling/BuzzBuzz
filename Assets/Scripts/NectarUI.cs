using UnityEngine;
using UnityEngine.UI;

public class NectarUI : MonoBehaviour
{
    public Image[] nectarSlots; // Assign your 5 images in order

    public void Start()
    {
        ClearNectarUI();
    }

    // Turn on the next nectar icon
    public void UpdateNectarUI(int currentNectar)
    {
        Debug.Log("Nectar UI Called with " +currentNectar + " Nectar");
        for (int i = 0; i < nectarSlots.Length; i++)
        {
            int index = nectarSlots.Length - 1 - i; // last element first
            nectarSlots[index].enabled = (i < currentNectar);
        }

    }

    // Turn all icons off
    public void ClearNectarUI()
    {
        Debug.Log("Clear Nectar Called!!!");
        foreach (var slot in nectarSlots)
        {
            slot.enabled = false;
        }
    }
}
