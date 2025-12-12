using UnityEngine;

public class ExitWardrobe : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) 
            return;

        if (UIFader.Instance != null) {
            UIFader.Instance.PlayWinFadeAndStop();
        }
    }
}
