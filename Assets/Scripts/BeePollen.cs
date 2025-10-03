using UnityEngine;

public class BeePollen : MonoBehaviour
{
    public bool hasPollen = false;
    public ParticleSystem pollenEffect;
    private AudioSource pollinateAudio;

    private void Awake()
    {
        // Get the AudioSource on the same GameObject
        pollinateAudio = GetComponent<AudioSource>();
    }

    public void PickupPollen()
    {
        hasPollen = true;

        if (pollenEffect != null)
        {
            pollenEffect.Play();
        }
    }

    public void DropPollen()
    {
        hasPollen = false;

        if (pollenEffect != null)
        {
            pollenEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        // Play sound when pollen is dropped (flower pollinated)
        if (pollinateAudio != null)
        {
            pollinateAudio.Play();
        }
    }
}
