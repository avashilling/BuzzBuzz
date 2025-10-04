using UnityEngine;

public class BeePollen : MonoBehaviour
{
    [Header("Pollen System")]
    public bool hasPollen = false;
    public ParticleSystem pollenEffect;
    private AudioSource pollinateAudio;

    [Header("Nectar System")]
    public int nectarCount = 0;
    public int maxNectar = 5;

    [Header("Optional UI")]
    public NectarUI nectarUI; // assign in inspector (optional)

    private void Awake()
    {
        pollinateAudio = GetComponent<AudioSource>();
    }

    // Called when picking up pollen from a flower
    public void PickupPollen()
    {
        hasPollen = true;
        if (pollenEffect != null) pollenEffect.Play();
    }

    // Called when dropping pollen onto a flower (flower calls this when pollinated)
    public void DropPollen()
    {
        hasPollen = false;

        if (pollenEffect != null)
            pollenEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (pollinateAudio != null)
            pollinateAudio.Play();

        // ---- IMPORTANT: gain nectar when we successfully pollinate a flower ----
        AddNectar();
    }

    // Adds one nectar (called at pollination). Caps at maxNectar.
    private void AddNectar()
    {
        if (nectarCount < maxNectar)
        {
            nectarCount++;
            Debug.Log($"Bee collected nectar! ({nectarCount}/{maxNectar})");
            nectarUI?.UpdateNectarUI(nectarCount);
        }
        else
        {
            Debug.Log("Bee nectar storage is full!");
        }
    }

    // Drop off nectar at the hive
    public int DropOffNectar()
    {
        int delivered = nectarCount;
        if (delivered > 0)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.AddNectarToTotal(delivered);
            else
                Debug.LogWarning("GameManager.Instance is null - cannot add nectar to total.");

            nectarCount = 0;
            nectarUI?.ClearNectarUI();
            Debug.Log($"Dropped off {delivered} nectar at the hive.");
        }

        return delivered;
    }
}
