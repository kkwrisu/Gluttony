using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [TextArea(3, 10)]
    public string[] dialogueLines;

    private bool playerInRange = false;

    public void Interact()
    {
        if (!playerInRange) return;

        if (dialogueManager.IsActive)
            dialogueManager.DisplayNextSentence();
        else
            dialogueManager.StartDialogue(dialogueLines);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }
}