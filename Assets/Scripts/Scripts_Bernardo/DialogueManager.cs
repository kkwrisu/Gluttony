using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Referência")]
    public PlayerController player;

    [Header("Configuração do Balão")]
    public Vector3 offset = new Vector3(0, 2f, 0);

    private Transform currentTarget;
    private Queue<string> sentences = new Queue<string>();
    private bool isDialogueActive = false;

    void Update()
    {
        // Faz o balão seguir o alvo (NPC ou Panela)
        if (isDialogueActive && currentTarget != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(currentTarget.position + offset);
            dialoguePanel.transform.position = screenPosition;
        }
    }

    public void StartDialogue(string[] lines, Transform target)
    {
        dialoguePanel.SetActive(true);
        isDialogueActive = true;
        currentTarget = target;

        if (player != null)
        {
            player.canMove = false;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }

        sentences.Clear();

        foreach (string sentence in lines)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = sentences.Dequeue();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
        currentTarget = null;

        if (player != null)
            player.canMove = true;
    }

    public bool IsActive => isDialogueActive;
}