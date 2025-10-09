using UnityEngine;
using System.Collections;

public class HiveTrigger : MonoBehaviour
{
    [Header("Nectar Drop Settings")]
    public GameObject nectarPrefab; // assign your nectar prefab
    public Transform dropPoint;     // assign the point it spawns from
    public float dropForce = 5f;    // optional force to give falling effect
    public float dropDelay = 0.1f;  // time between each nectar drop

    private void OnTriggerEnter(Collider other)
    {
        var bee = other.GetComponent<BeePollen>();
        if (bee != null)
        {
            int deposited = bee.DropOffNectar();
            if (deposited > 0)
            {
                Debug.Log($"Hive accepted {deposited} nectar.");
                StartCoroutine(SpawnNectarDropsSequentially(deposited));
            }
        }
    }

    private IEnumerator SpawnNectarDropsSequentially(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnNectarDrop();
            yield return new WaitForSeconds(dropDelay);
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
