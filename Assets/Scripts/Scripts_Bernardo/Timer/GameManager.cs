using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public TMP_Text timerText; // Arraste seu texto de relógio aqui
    
    [Header("Configurações")]
    public float timeRemaining = 180f;
    public int itensEntregues = 0;
    public int metaEntregas = 3;
    
    private bool isPaused = false;

    void Update()
    {
        // Pausa o timer se o diálogo estiver ativo
        isPaused = (dialogueManager != null && dialogueManager.IsActive);

        if (!isPaused && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }

        if (timeRemaining <= 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int min = Mathf.FloorToInt(timeRemaining / 60);
            int sec = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    public void IncrementarEntregas()
    {
        itensEntregues++;
        
        if (itensEntregues >= metaEntregas)
        {
            SceneManager.LoadScene("Vitoria");
        }
    }
}
