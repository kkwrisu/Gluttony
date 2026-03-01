using UnityEngine;
using System.Collections;

public class CollectableItem : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;

    public IngredientType itemType;

    [Header("Ping Settings")]
    public float pingDuration = 0.6f;
    public int pingCount = 3;

    private bool canBeCollected = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void PickUp(Transform carryTransform)
    {
        if (rb != null)
        {
            rb.simulated = false;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (col != null)
            col.enabled = false;

        transform.SetParent(carryTransform);
        transform.position = carryTransform.position;

        canBeCollected = true;
    }

    public void Drop(bool usePing = true)
    {
        transform.SetParent(null);

        if (rb != null)
            rb.simulated = true;

        if (col != null)
            col.enabled = true;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (usePing)
            StartCoroutine(Ping());
        else
            canBeCollected = true;
    }

    public void PlaceOnSurface(Transform parent)
    {
        if (rb != null)
        {
            rb.simulated = false;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (col != null)
            col.enabled = false;

        transform.SetParent(parent);
        transform.position = parent.position;

        canBeCollected = true;
    }

    private IEnumerator Ping()
    {
        if (sr == null)
            yield break;

        canBeCollected = false;

        float singlePingTime = pingDuration / pingCount;

        for (int i = 0; i < pingCount; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(singlePingTime);
        }

        sr.enabled = true;
        canBeCollected = true;
    }

    public bool CanBePickedUp()
    {
        return canBeCollected;
    }
}