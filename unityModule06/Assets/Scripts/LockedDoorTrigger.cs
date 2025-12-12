using UnityEngine;

public class LockedDoorTrigger : MonoBehaviour
{
    public Door door;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.TryOpen();
        }
    }
}
