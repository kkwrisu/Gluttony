using UnityEngine;

public class Workbench : MonoBehaviour
{
    public Transform placePoint;

    private CollectableItem storedItem;

    public void Interact(PlayerCarrying player)
    {
        if (player == null) return;

        bool playerHasItem = player.IsCarryingItem();

        if (playerHasItem && storedItem == null)
        {
            CollectableItem item = player.GetCarriedItem();
            storedItem = item;

            player.ClearCarriedItem();
            item.PlaceOnSurface(placePoint);

            return;
        }

        if (!playerHasItem && storedItem != null)
        {
            CollectableItem item = storedItem;
            storedItem = null;

            player.ForcePickUp(item);

            return;
        }

        if (playerHasItem && storedItem != null)
        {
            CollectableItem playerItem = player.GetCarriedItem();
            CollectableItem benchItem = storedItem;

            player.ClearCarriedItem();
            playerItem.PlaceOnSurface(placePoint);

            player.ForcePickUp(benchItem);

            storedItem = playerItem;
        }
    }
}