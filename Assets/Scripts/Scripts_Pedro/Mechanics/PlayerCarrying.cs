using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarrying : MonoBehaviour
{
    public Transform carryPoint;
    public float interactRange = 1f;
    public LayerMask interactLayer;

    private PlayerInput playerInput;
    private CollectableItem carriedItem;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.actions["Interact"].WasPressedThisFrame())
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, interactLayer);
        if (hit == null) return;

        CookingPot pot = hit.GetComponent<CookingPot>();
        if (pot != null)
        {
            pot.Interact(this);
            return;
        }

        if (carriedItem == null)
        {
            CollectableItem item = hit.GetComponent<CollectableItem>();
            if (item != null && item.CanBePickedUp())
            {
                carriedItem = item;
                item.PickUp(carryPoint);
            }
        }
    }

    public void TryCollectSpecific(CollectableItem item)
    {
        if (item == null || !item.CanBePickedUp()) return;
        if (carriedItem != null) return;

        carriedItem = item;
        item.PickUp(carryPoint);
    }

    public void DropItem()
    {
        if (carriedItem == null) return;

        carriedItem.Drop();
        carriedItem = null;
    }

    public bool IsCarryingItem()
    {
        return carriedItem != null;
    }

    public CollectableItem GetCarriedItem()
    {
        return carriedItem;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}