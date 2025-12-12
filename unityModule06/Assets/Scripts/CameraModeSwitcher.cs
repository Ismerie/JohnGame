using UnityEngine;
using UnityEngine.InputSystem;

public class CameraModeSwitcher : MonoBehaviour
{
    public GameObject tpsMainCamera;
    public GameObject fpsCamera;

    bool isFPS = false;

    void Start() => SetMode(false);

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
            SetMode(!isFPS);
    }

    void SetMode(bool fps)
    {
        isFPS = fps;

        fpsCamera.SetActive(fps);
        tpsMainCamera.SetActive(!fps);

        var p = FindFirstObjectByType<PlayerViewController>();
        if (p != null) p.SetFPS(fps);
    }
}
