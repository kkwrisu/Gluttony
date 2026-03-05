using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    [Header("Referências UI")]
    public GameObject caixaDialogo;
    public GameObject textoDialogo;
    public TextMeshProUGUI dialogueText;

    [Header("Referência Player")]
    public PlayerController player;

    private Queue<string> sentences = new Queue<string>();
    private bool isDialogueActive = false;

    private void Start()
    {
        if (caixaDialogo != null)
            caixaDialogo.SetActive(false);
    }

    public void StartDialogue(string[] lines)
    {
        caixaDialogo.SetActive(true);
        textoDialogo.SetActive(true);

        isDialogueActive = true;

        if (player != null)
        {
            player.canMove = false;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = Vector2.zero;
        }

        sentences.Clear();

        foreach (string sentence in lines)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (!isDialogueActive)
            return;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = sentences.Dequeue();
    }

    private void EndDialogue()
    {
        caixaDialogo.SetActive(false);
        textoDialogo.SetActive(false);
        isDialogueActive = false;

        if (player != null)
            player.canMove = true;
    }

    public bool IsActive => isDialogueActive;
}