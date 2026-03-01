using UnityEngine;
using TMPro; // Se estiver usando TextMeshPro
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    
    private Queue<string> sentences = new Queue<string>();
    private bool isDialogueActive = false;

    public void StartDialogue(string[] lines)
    {
        dialoguePanel.SetActive(true);
        isDialogueActive = true;
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
    }

    public bool IsActive => isDialogueActive;
}
