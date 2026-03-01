using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Pedido:")]
    public IngredientType requestedSoup;

    [TextArea(3, 5)]
    public string[] requestDialogue;

    [TextArea(3, 5)]
    public string[] successDialogue;

    [TextArea(3, 5)]
    public string[] wrongItemDialogue;

    [Header("Recompensa opcional:")]
    public CollectableItem meatPrefab;

    private bool playerInRange = false;
    private bool orderCompleted = false;
    private bool rewardPending = false;

    public void Interact(PlayerCarrying player)
    {
        if (!playerInRange || player == null)
            return;

        if (dialogueManager.IsActive)
        {
            dialogueManager.DisplayNextSentence();

            if (!dialogueManager.IsActive && rewardPending)
            {
                GiveReward(player);
                rewardPending = false;
            }

            return;
        }

        if (orderCompleted)
        {
            dialogueManager.StartDialogue(successDialogue);
            return;
        }

        if (player.IsCarryingItem())
        {
            CollectableItem heldItem = player.GetCarriedItem();

            if (heldItem != null && heldItem.itemType == requestedSoup)
            {
                player.DropItem();
                Destroy(heldItem.gameObject);

                orderCompleted = true;
                rewardPending = true;

                dialogueManager.StartDialogue(successDialogue);
            }
            else
            {
                dialogueManager.StartDialogue(wrongItemDialogue);
            }

            return;
        }

        dialogueManager.StartDialogue(requestDialogue);
    }

    private void GiveReward(PlayerCarrying player)
    {
        if (meatPrefab == null) return;

        CollectableItem meat = Instantiate(meatPrefab);

        player.ForcePickUp(meat);
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