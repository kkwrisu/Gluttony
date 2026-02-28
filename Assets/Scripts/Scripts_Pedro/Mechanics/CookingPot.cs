using UnityEngine;
using System.Collections;

public class CookingPot : MonoBehaviour
{
    [Header("Configurações")]
    public float cookingTime = 30f;
    public SpriteRenderer visualIndicator;
    public CollectableItem soupPrefab;

    private bool isCooking = false;
    private bool isReady = false;
    private string soupType;

    private void Start()
    {
        if (visualIndicator != null)
            visualIndicator.enabled = false;
    }

    public void Interact(PlayerCarrying playerCarry)
    {
        if (isCooking || playerCarry == null)
            return;

        if (playerCarry.IsCarryingItem())
        {
            CollectableItem heldItem = playerCarry.GetCarriedItem();

            if (heldItem != null)
            {
                soupType = DetermineSoupType(heldItem);

                playerCarry.DropItem();
                Destroy(heldItem.gameObject);

                StartCoroutine(CookSoup());
            }
        }
        else if (isReady)
        {
            SpawnSoup(playerCarry);
        }
    }

    private string DetermineSoupType(CollectableItem item)
    {
        string itemName = item.gameObject.name.ToLower();

        if (itemName.Contains("tomate"))
            return "Tomate";
        else if (itemName.Contains("batata"))
            return "Batata";
        else
            return "Carne";
    }

    private IEnumerator CookSoup()
    {
        isCooking = true;
        isReady = false;

        if (visualIndicator != null)
            visualIndicator.enabled = true;

        yield return new WaitForSeconds(cookingTime);

        isCooking = false;
        isReady = true;

        if (visualIndicator != null)
        {
            visualIndicator.color = Color.green;
        }
    }

    private void SpawnSoup(PlayerCarrying playerCarry)
    {
        if (!isReady || soupPrefab == null)
            return;

        CollectableItem soup = Instantiate(soupPrefab, transform.position + Vector3.up, Quaternion.identity);

        playerCarry.TryCollectSpecific(soup);

        isReady = false;

        if (visualIndicator != null)
            visualIndicator.enabled = false;
    }
}