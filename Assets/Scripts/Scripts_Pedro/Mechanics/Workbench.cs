using UnityEngine;

public class Workbench : MonoBehaviour
{
    public Transform placePoint;

    [Header("Prefab Inicial (Opcional)")]
    [SerializeField] private CollectableItem startingItemPrefab;

    private CollectableItem storedItem;

    private void Start()
    {
        if (startingItemPrefab != null)
        {
            CollectableItem instance = Instantiate(startingItemPrefab);
            storedItem = instance;
            instance.PlaceOnSurface(placePoint);
        }
    }

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