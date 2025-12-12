using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Rotation")]
    public Transform doorMesh;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    public int requiredKeys = 3;

    private bool isOpen = false;
    private Quaternion closedRot;
    private Quaternion openRot;

    void Start()
    {
        if (doorMesh == null)
            doorMesh = transform;

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

    public void TryOpen()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.keys >= requiredKeys)
        {
            isOpen = true;
        }
        else
        {
            Debug.Log("Porte verrouillée : il manque des clés !");
        }
    }
}
