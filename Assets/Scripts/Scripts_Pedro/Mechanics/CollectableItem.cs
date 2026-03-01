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

    private void FixedUpdate()
    {
        if (rb != null && rb.simulated && rb.linearVelocity.magnitude < 0.05f)
            rb.linearVelocity = Vector2.zero;
    }

    public void PickUp(Transform carryTransform)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
        }

        if (col != null)
            col.enabled = false;

        transform.position = carryTransform.position;
        transform.SetParent(carryTransform);

        canBeCollected = true;

        if (sr != null)
            sr.enabled = true;
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