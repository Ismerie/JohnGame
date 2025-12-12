using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIFader : MonoBehaviour
{
    public static UIFader Instance;

    [Header("Refs")]
    public Image fadeImage;
    public CanvasGroup canvasGroup;

    [Header("Sprites")]
    public Sprite caughtSprite;
    public Sprite winSprite;

    [Header("Caught Timing")]
    public float caughtFadeIn = 0.6f;
    public float caughtHold = 1.2f;
    public float caughtFadeOut = 0.6f;

    [Header("Win Timing")]
    public float winFadeIn = 0.6f;

    private bool busy = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (fadeImage != null)
        {
            fadeImage.raycastTarget = false;
            fadeImage.enabled = false;
        }

        if (canvasGroup == null && fadeImage != null)
            canvasGroup = fadeImage.GetComponent<CanvasGroup>();

        if (canvasGroup != null)
            canvasGroup.alpha = 0f;


        Time.timeScale = 1f;
    }

    public void PlayCaughtFadeAndRestart()
    {
        if (busy) return;
        busy = true; 

        Time.timeScale = 1f;

        DisablePlayerControl();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayFaint();


        StopAllCoroutines();
        StartCoroutine(CaughtRoutine());
    }

    IEnumerator CaughtRoutine()
    {
        yield return FadeInHoldOut(caughtSprite, caughtFadeIn, caughtHold, caughtFadeOut);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
        busy = false;
    }

    public void PlayWinFadeAndStop()
    {
        if (busy) return;
        busy = true;
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayWin();

        StopAllCoroutines();
        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine()
    {

        yield return FadeInStay(winSprite, winFadeIn);

        DisablePlayerControl();

        Time.timeScale = 0f;


    }

    void DisablePlayerControl()
    {

        var p = FindFirstObjectByType<PlayerViewController>();
        if (p != null) p.enabled = false;


        var jm = FindFirstObjectByType<JohnMovement>();
        if (jm != null) jm.enabled = false;
    }

    IEnumerator FadeInHoldOut(Sprite sprite, float fadeIn, float hold, float fadeOut)
    {
        if (!Prepare(sprite)) yield break;

        yield return Fade(0f, 1f, fadeIn);

        if (hold > 0f)
            yield return new WaitForSecondsRealtime(hold);

        yield return Fade(1f, 0f, fadeOut);

        fadeImage.enabled = false;
    }

    IEnumerator FadeInStay(Sprite sprite, float fadeIn)
    {
        if (!Prepare(sprite)) yield break;

        yield return Fade(0f, 1f, fadeIn);

        // reste affiché
        canvasGroup.alpha = 1f;
        fadeImage.enabled = true;
    }

    bool Prepare(Sprite sprite)
    {
        if (fadeImage == null || canvasGroup == null || sprite == null)
            return false;

        fadeImage.sprite = sprite;
        fadeImage.enabled = true;
        canvasGroup.alpha = 0f;
        return true;
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        if (duration <= 0f)
        {
            canvasGroup.alpha = to;
            yield break;
        }

        float t = 0f;
        canvasGroup.alpha = from;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}
