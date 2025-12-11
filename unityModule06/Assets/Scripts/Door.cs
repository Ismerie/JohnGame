using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Rotation")]
    public Transform doorMesh;        // ce qu'on va faire tourner (souvent Pivot lui-même)
    public float openAngle = 90f;    // angle d'ouverture
    public float openSpeed = 2f;     // vitesse d'animation

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        if (doorMesh == null)
            doorMesh = transform; // par défaut, on tourne Pivot

        closedRot = doorMesh.localRotation;
        openRot = closedRot * Quaternion.Euler(0f, openAngle, 0f);
    }

    void Update()
    {
        if (isOpen)
        {
            doorMesh.localRotation = Quaternion.Lerp(
                doorMesh.localRotation,
                openRot,
                Time.deltaTime * openSpeed
            );
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }
}
