using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    [TextArea(3, 10)]
    public string[] dialogueLines; // Você escreve os textos no Inspector da Unity!

    private bool playerInRange = false;

    void Update()
    {
        // Se o player estiver perto e apertar Q
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            if (dialogueManager.IsActive)
            {
                dialogueManager.DisplayNextSentence();
            }
            else
            {
                dialogueManager.StartDialogue(dialogueLines);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            // Opcional: fechar diálogo se o player sair de perto
        }
    }
}
