using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    [Header("Telas")]
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject controlsScreen;
    [SerializeField] private GameObject creditsScreen;

    [Header("Transição (opcional)")]
    [SerializeField] private CanvasGroup fadeCanvas;
    [SerializeField] private float fadeSpeed = 1.5f;

    [Header("Cena de Gameplay")]
    [SerializeField] private string gameplaySceneName = "Fase_1";

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        mainScreen.SetActive(true);
        controlsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.gameObject.SetActive(false);
        }
    }

    public void BotJogar()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        if (fadeCanvas != null)
            StartCoroutine(FadeAndLoad());
        else
            SceneManager.LoadScene(gameplaySceneName);
    }

    private IEnumerator FadeAndLoad()
    {
        fadeCanvas.gameObject.SetActive(true);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            fadeCanvas.alpha = Mathf.Clamp01(t);
            yield return null;
        }

        SceneManager.LoadScene(gameplaySceneName);
    }

    public void BotControles()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        mainScreen.SetActive(false);
        controlsScreen.SetActive(true);
    }

    public void BotCreditos()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        mainScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void BotVoltar()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        controlsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void BotSair()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        Application.Quit();
    }
}