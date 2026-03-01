using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Referência")]
    public PlayerController player;

    private Queue<string> sentences = new Queue<string>();
    private bool isDialogueActive = false;

    public void StartDialogue(string[] lines)
    {
        dialoguePanel.SetActive(true);
        isDialogueActive = true;

        if (player != null)
        {
            player.canMove = false;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
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

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isDialogueActive = false;

        if (player != null)
            player.canMove = true;
    }

    public bool IsActive => isDialogueActive;
}