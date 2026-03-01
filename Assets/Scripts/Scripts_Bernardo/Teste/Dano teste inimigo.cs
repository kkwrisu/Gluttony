using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    public int damage = 1;

    // Quando algo colide com o inimigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto que tocou é o Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Tenta pegar o script PlayerHealth e aplica o dano
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                playerHealth.ChangeHealth(-damage);
                Debug.Log("Player tocou no inimigo e levou dano!");
            }
        }
    }
}