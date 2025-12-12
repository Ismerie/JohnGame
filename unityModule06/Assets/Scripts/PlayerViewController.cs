using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerViewController : MonoBehaviour
{
    public Transform fpsCameraTransform;
    public Transform tpsCameraTransform;
    
    public float stepInterval = 0.45f;
    float stepTimer;

    public Animator animator;
    public string speedParam = "Speed";
    Vector3 lastPos;

    public float moveSpeed = 4f;
    public float gravity = -20f;

    public float tpsSpeed = 2.5f;
    public float fpsSpeed = 3f;

    public float mouseSensitivity = 0.12f;
    public float pitchMin = -70f;
    public float pitchMax = 70f;

    CharacterController cc;
    bool isFPS = false;
    float pitch = 0f;
    Vector3 verticalVel;

    void Awake()
    {
        if (animator == null) 
            animator = GetComponentInChildren<Animator>();
        lastPos = transform.position;

        cc = GetComponent<CharacterController>();
        if (tpsCameraTransform == null && Camera.main != null) tpsCameraTransform = Camera.main.transform;
    }

    public void SetFPS(bool fps)
    {
        isFPS = fps;
        Cursor.lockState = fps ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !fps;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        if (isFPS) 
            FPSUpdate();
        else 
            TPSUpdate();
        Vector3 delta = (transform.position - lastPos);
        delta.y = 0f;
        float speed = delta.magnitude / Mathf.Max(Time.deltaTime, 0.0001f);


    }

    Vector2 ReadMove()
    {
        float x = 0f, y = 0f;

        if (Keyboard.current.aKey.isPressed || Keyboard.current.qKey.isPressed) x -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;

        if (Keyboard.current.wKey.isPressed || Keyboard.current.zKey.isPressed) y += 1f;
        if (Keyboard.current.sKey.isPressed) y -= 1f;

        return new Vector2(x, y);
    }

    void TPSUpdate()
    {
        Vector2 input = ReadMove();

        if (input.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
            stepTimer = 0f;

        if (animator != null)
            animator.SetFloat("Speed", input.magnitude);

        Transform cam = tpsCameraTransform != null ? tpsCameraTransform : transform;
        Vector3 forward = cam.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = cam.right; right.y = 0f; right.Normalize();

        Vector3 move = (right * input.x + forward * input.y);
        if (move.sqrMagnitude > 1f) move.Normalize();
        ApplyMove(move * tpsSpeed);

        if (move.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(move), Time.deltaTime * 12f);
    }

    void FPSUpdate()
    {

        if (Mouse.current != null && fpsCameraTransform != null)
        {
            Vector2 delta = Mouse.current.delta.ReadValue() * mouseSensitivity;

            transform.Rotate(0f, delta.x, 0f);

            pitch -= delta.y;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
            fpsCameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        Vector2 input = ReadMove();

        if (input.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (AudioManager.Instance != null)
                    AudioManager.Instance.PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
            stepTimer = 0f;

        if (animator != null)
            animator.SetFloat("Speed", input.magnitude);

        Vector3 forward = transform.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = transform.right; right.y = 0f; right.Normalize();

        Vector3 move = (right * input.x + forward * input.y);
        if (move.sqrMagnitude > 1f) 
            move.Normalize();
        ApplyMove(move * fpsSpeed);
    }

    void ApplyMove(Vector3 horizontal)
    {
        if (cc.isGrounded && verticalVel.y < 0f) verticalVel.y = -1f;
        verticalVel.y += gravity * Time.deltaTime;
        cc.Move((horizontal + verticalVel) * Time.deltaTime);
    }
}
