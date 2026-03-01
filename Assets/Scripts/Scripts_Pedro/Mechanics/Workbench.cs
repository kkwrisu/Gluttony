using UnityEngine;

public class Workbench : MonoBehaviour
{
    public Transform placePoint;

    private CollectableItem storedItem;

    public void Interact(PlayerCarrying player)
    {
        if (player == null) return;

        if (player.IsCarryingItem())
        {
            if (storedItem != null) return;

            CollectableItem item = player.GetCarriedItem();
            storedItem = item;

            player.ClearCarriedItem();

            item.PlaceOnSurface(placePoint);
        }
        else
        {
            if (storedItem == null) return;

            CollectableItem item = storedItem;
            storedItem = null;

            player.ForcePickUp(item);
        }
    }
}