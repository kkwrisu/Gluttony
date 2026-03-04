using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public TMP_Text timerText;

    [Header("Configurações")]
    [Tooltip("Tempo inicial em segundos. Pode ser alterado pelo Inspector.")]
    public float timeRemaining = 180f; // Valor padrão, mas o do Inspector prevalecerá
    
    public int itensEntregues = 0;
    public int metaEntregas = 3;

    private bool isPaused = false;
    private bool gameEnded = false;

    void Start()
    {
        // Garante que a UI comece com o tempo definido no Inspector
        UpdateTimerUI();
    }

    void Update()
    {
        if (gameEnded) return;

        // Verifica se o diálogo está ativo para pausar o cronômetro
        isPaused = (dialogueManager != null && dialogueManager.IsActive);

        if (!isPaused)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                
                // Impede que o tempo fique negativo
                if (timeRemaining < 0)
                    timeRemaining = 0;

                UpdateTimerUI();
            }
            else if (!gameEnded) // Se o tempo chegou a 0
            {
                gameEnded = true;
                UpdateTimerUI();
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            // O cálculo garante que o tempo exibido respeite o valor exato de timeRemaining
            int min = Mathf.FloorToInt(timeRemaining / 60);
            int sec = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    public void IncrementarEntregas()
    {
        if (gameEnded) return;

        itensEntregues++;

        if (itensEntregues >= metaEntregas)
        {
            gameEnded = true;
            SceneManager.LoadScene("Vitoria");
        }
    }
}