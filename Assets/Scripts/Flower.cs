using System;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public bool hasPollen = true;
    public bool pollinated = false;
    private bool canBePollinated = true;

    public ParticleSystem pollenEffect;

    [Header("Petal Models")]
    public GameObject closedPetals;  // closed version
    public GameObject petals;        // open version

    void Start()
    {
        if (hasPollen)
        {
            // Flower already has pollen at start
            canBePollinated = false;

            // Show only the open petals
            petals?.SetActive(true);
            closedPetals?.SetActive(false);

            pollenEffect?.Play();
        }
        else
        {
            // No pollen, but can be pollinated
            canBePollinated = true;

            // Start with closed petals
            petals?.SetActive(false);
            closedPetals?.SetActive(true);
        }
    }

    public void Interact(BeePollen bee)
    {
        if (!bee.hasPollen && hasPollen)
        {
            // Bee picks up pollen
            bee.PickupPollen();
            hasPollen = false;

            pollenEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            Debug.Log("Pollen has been picked up from flower!");
        }
        else if (bee.hasPollen && !pollinated && canBePollinated)
        {
            // Bee delivers pollen
            bee.DropPollen();
            pollinated = true;

            GameManager.Instance.OnFlowerPollinated();
            Debug.Log("Flower has been pollinated!");

            // Switch to open petals
            petals?.SetActive(true);
            closedPetals?.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var bee = other.GetComponent<BeePollen>();
        if (bee != null)
        {
            Interact(bee);
        }
    }
}
