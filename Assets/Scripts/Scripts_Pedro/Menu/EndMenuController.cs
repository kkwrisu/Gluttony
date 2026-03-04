using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenuController : MonoBehaviour
{
    [Header("Cena do Menu Principal")]
    [SerializeField] private string mainMenuSceneName = "MenuInicial";

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;
    }

    public void BotRecomecar()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BotMenu()
    {
        if (audioManager != null)
            audioManager.PlayButtonClick();

        SceneManager.LoadScene(mainMenuSceneName);
    }
}