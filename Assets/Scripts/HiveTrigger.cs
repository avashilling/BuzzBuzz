using UnityEngine;

public class HiveTrigger : MonoBehaviour
{
    [Header("Nectar Drop Settings")]
    public GameObject nectarPrefab; // assign your nectar prefab
    public Transform dropPoint;     // assign the point it spawns from
    public float dropForce = 5f;    // optional force to give falling effect

    private void OnTriggerEnter(Collider other)
    {
        var bee = other.GetComponent<BeePollen>();
        if (bee != null)
        {
            int deposited = bee.DropOffNectar();
            if (deposited > 0)
            {
                // optional: play hive sound, particle, UI update, etc.
                Debug.Log($"Hive accepted {deposited} nectar.");

                // Spawn nectar drop prefab for each nectar collected
                for (int i = 0; i < deposited; i++)
                {
                    SpawnNectarDrop();
                }
            }
        }
    }

    private void SpawnNectarDrop()
    {
        if (nectarPrefab == null || dropPoint == null)
        {
            Debug.LogWarning("Nectar prefab or drop point not assigned!");
            return;
        }

        GameObject nectar = Instantiate(nectarPrefab, dropPoint.position, Quaternion.identity);

        // Optional: add Rigidbody force for falling effect
        Rigidbody rb = nectar.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.down * dropForce, ForceMode.Impulse);
        }
    }
}
