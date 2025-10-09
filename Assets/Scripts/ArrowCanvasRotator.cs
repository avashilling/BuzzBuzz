using UnityEngine;

public class ArrowCanvasRotator : MonoBehaviour
{
    public Transform player; // optional if arrow canvas is already on player
    public Transform target;

    private void LateUpdate()
    {
        if (target == null) return;

        // Vector from arrow to target
        Vector3 direction = target.position - transform.position;

        // Keep upright, rotate only around Y
        direction.y = 0;

        if (direction.sqrMagnitude < 0.001f) return;

        // Rotate arrow to face target
        transform.rotation = Quaternion.LookRotation(direction);

        // Optional: adjust rotation if arrow image points right/left instead of forward
        transform.Rotate(0f, 0f, 90f);
    }
}
