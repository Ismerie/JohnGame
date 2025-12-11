using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public Door door;   // référence vers le script sur Pivot

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.OpenDoor();
        }
    }
}
