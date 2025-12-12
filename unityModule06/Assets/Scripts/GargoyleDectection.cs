using UnityEngine;

public class GargoyleDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Torchlight: Player detected");

        if (GhostManager.Instance == null)
        {
            Debug.LogError("GhostManager.Instance is NULL (pas de GhostManager actif en scène)");
            return;
        }

        Debug.Log("GhostManager found -> AlertAll()");
        GhostManager.Instance.AlertAll(other.transform);
    }
}
