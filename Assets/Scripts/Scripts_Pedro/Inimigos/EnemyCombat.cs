using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Ataque")]
    public int damage = 1;
    public float attackRange = 1.5f;
    public LayerMask playerLayer;

    public void Attack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, playerLayer);

        foreach (Collider2D hit in hits)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.ChangeHealth(-damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}