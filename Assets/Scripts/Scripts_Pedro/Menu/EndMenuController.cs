using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class EndGameMenuController : MonoBehaviour
{
    [Header("Cena do Menu Principal")]
    [SerializeField] private string mainMenuSceneName = "MenuInicial";

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeSpeed = 1.5f;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.gameObject.SetActive(false);
        }
    }

    public void BotRecomecar()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        StartCoroutine(FadeAndRestart());
    }

    public void BotMenu()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        StartCoroutine(FadeAndLoadMenu());
    }

    private IEnumerator FadeAndRestart()
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator FadeAndLoadMenu()
    {
        yield return StartCoroutine(FadeOut());
        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator FadeOut()
    {
        if (fadeCanvas == null)
            yield break;

        fadeCanvas.gameObject.SetActive(true);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            fadeCanvas.alpha = Mathf.Clamp01(t);
            yield return null;
        }
    }
}