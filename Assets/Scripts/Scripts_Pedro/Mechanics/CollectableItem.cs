using UnityEngine;
using System.Collections;

public class CollectableItem : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;

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
        if (rb.simulated && rb.linearVelocity.magnitude < 0.05f)
            rb.linearVelocity = Vector2.zero;
    }

    public void PickUp(Transform carryTransform)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.simulated = false;
        col.enabled = false;

        transform.position = carryTransform.position;
        transform.SetParent(carryTransform);

        canBeCollected = true;
        sr.enabled = true;
    }

    public void Drop()
    {
        transform.SetParent(null);
        rb.simulated = true;
        col.enabled = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        StartCoroutine(Ping());
    }

    private IEnumerator Ping()
    {
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