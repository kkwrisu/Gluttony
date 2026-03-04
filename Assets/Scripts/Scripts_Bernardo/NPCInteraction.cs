using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public GameManager gameManager;

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

    private bool orderCompleted = false;
    private bool rewardPending = false;

    public void Interact(PlayerCarrying player)
    {
        if (player == null)
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
            dialogueManager.StartDialogue(successDialogue, transform);
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

                if (gameManager != null)
                    gameManager.IncrementarEntregas();

                dialogueManager.StartDialogue(successDialogue, transform);
            }
            else
            {
                dialogueManager.StartDialogue(wrongItemDialogue, transform);
            }

            return;
        }

        dialogueManager.StartDialogue(requestDialogue, transform);
    }

    private void GiveReward(PlayerCarrying player)
    {
        if (meatPrefab == null)
            return;

        CollectableItem meat = Instantiate(meatPrefab);
        player.ForcePickUp(meat);
    }
}