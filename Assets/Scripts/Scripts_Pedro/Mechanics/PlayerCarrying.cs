using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCarrying : MonoBehaviour
{
    [Header("Referências")]
    public Transform carryPoint;

    [Header("Interação")]
    public float interactRange = 1f;
    public LayerMask interactLayer;
    public float interactCooldown = 0.3f;

    [Header("Drop Settings")]
    public float dropBlockTime = 0.2f;

    private PlayerInput playerInput;
    private CollectableItem carriedItem;

    private float lastInteractTime;
    private float blockInteractUntil;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.actions["Interact"].WasPressedThisFrame())
        {
            if (Time.time >= lastInteractTime + interactCooldown &&
                Time.time >= blockInteractUntil)
            {
                lastInteractTime = Time.time;
                TryInteract();
            }
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

        Workbench bench = hit.GetComponent<Workbench>();
        if (bench != null)
        {
            bench.Interact(this);
            return;
        }

        NPCInteraction npc = hit.GetComponent<NPCInteraction>();
        if (npc != null)
        {
            npc.Interact();
            return;
        }

        CollectableItem item = hit.GetComponent<CollectableItem>();
        if (item == null || !item.CanBePickedUp())
            return;

        if (carriedItem == null)
        {
            carriedItem = item;
            item.PickUp(carryPoint);
        }
        else
        {
            SwapItem(item);
        }
    }

    private void SwapItem(CollectableItem worldItem)
    {
        if (worldItem == null || carriedItem == null)
            return;

        CollectableItem oldItem = carriedItem;

        Vector3 worldPosition = worldItem.transform.position;

        oldItem.transform.position = worldPosition;
        oldItem.Drop();

        blockInteractUntil = Time.time + dropBlockTime;

        carriedItem = worldItem;
        worldItem.PickUp(carryPoint);
    }

    public void ForcePickUp(CollectableItem item)
    {
        if (item == null) return;

        carriedItem = item;
        item.PickUp(carryPoint);
    }

    public void ClearCarriedItem()
    {
        carriedItem = null;
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

        blockInteractUntil = Time.time + dropBlockTime;
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