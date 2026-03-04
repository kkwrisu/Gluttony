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
        Debug.Log("=== MAIN MENU START ===");

        audioManager = AudioManager.Instance;

        mainScreen.SetActive(true);
        controlsScreen.SetActive(false);
        creditsScreen.SetActive(false);

        Debug.Log("Main ativo: " + mainScreen.activeSelf);
        Debug.Log("Controls ativo: " + controlsScreen.activeSelf);
        Debug.Log("Credits ativo: " + creditsScreen.activeSelf);

        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
            fadeCanvas.gameObject.SetActive(false);
            Debug.Log("Fade configurado e desativado.");
        }
        else
        {
            Debug.Log("FadeCanvas é NULL.");
        }
    }

    public void BotJogar()
    {
        Debug.Log("Botão JOGAR clicado.");

        if (audioManager != null)
            audioManager.PlayButtonClick();
        else
            Debug.Log("AudioManager é NULL!");

        if (fadeCanvas != null)
            StartCoroutine(FadeAndLoad());
        else
            SceneManager.LoadScene(gameplaySceneName);
    }

    private IEnumerator FadeAndLoad()
    {
        Debug.Log("Iniciando Fade...");

        fadeCanvas.gameObject.SetActive(true);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            fadeCanvas.alpha = Mathf.Clamp01(t);
            yield return null;
        }

        Debug.Log("Fade completo. Carregando cena: " + gameplaySceneName);
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void BotControles()
    {
        Debug.Log("Botão CONTROLES clicado.");

        if (audioManager != null)
            audioManager.PlayButtonClick();
        else
            Debug.Log("AudioManager é NULL!");

        mainScreen.SetActive(false);
        controlsScreen.SetActive(true);

        Debug.Log("Main ativo: " + mainScreen.activeSelf);
        Debug.Log("Controls ativo: " + controlsScreen.activeSelf);
    }

    public void BotCreditos()
    {
        Debug.Log("Botão CRÉDITOS clicado.");

        if (audioManager != null)
            audioManager.PlayButtonClick();
        else
            Debug.Log("AudioManager é NULL!");

        mainScreen.SetActive(false);
        creditsScreen.SetActive(true);

        Debug.Log("Main ativo: " + mainScreen.activeSelf);
        Debug.Log("Credits ativo: " + creditsScreen.activeSelf);
    }

    public void BotVoltar()
    {
        Debug.Log("Botão VOLTAR clicado.");

        if (audioManager != null)
            audioManager.PlayButtonClick();
        else
            Debug.Log("AudioManager é NULL!");

        controlsScreen.SetActive(false);
        creditsScreen.SetActive(false);
        mainScreen.SetActive(true);

        Debug.Log("Main ativo: " + mainScreen.activeSelf);
        Debug.Log("Controls ativo: " + controlsScreen.activeSelf);
        Debug.Log("Credits ativo: " + creditsScreen.activeSelf);
    }

    public void BotSair()
    {
        Debug.Log("Botão SAIR clicado.");

        if (audioManager != null)
            audioManager.PlayButtonClick();
        else
            Debug.Log("AudioManager é NULL!");

        Application.Quit();
    }
}