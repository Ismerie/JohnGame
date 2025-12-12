using UnityEngine;

public class GhostDetection : MonoBehaviour
{
    public GhostAI ghost;

    void Awake()
    {
        if (ghost == null)
            ghost = GetComponentInParent<GhostAI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ghost.DetectPlayer(other.transform);
        }
    }
}
