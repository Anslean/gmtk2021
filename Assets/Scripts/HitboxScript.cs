using UnityEngine;
using static Direction;
using UnityEngine.SceneManagement;

public class HitboxScript : MonoBehaviour
{
    private int damage = 0;
    private bool perpetuatedAttack = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GolemController player;
        if (collider.gameObject.TryGetComponent<GolemController>(out player))
        {
            if (player.isAttacking && !perpetuatedAttack)
            {
                // Player killed it!
                damage++;
                GetComponent<BoxCollider2D>().enabled = false;
                if (damage == 1)
                    transform.parent.GetComponent<SpriteRenderer>().color = Color.yellow;
                else if (damage == 2)
                    transform.parent.GetComponent<SpriteRenderer>().color = Color.red;
                player.makeInvincibleToBoss();
                perpetuatedAttack = true;
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
            else if (!player.isAttacking && !player.isInvincibleToBoss())
                // Player got kill'd by it!
                player.Die();
            if (damage == 3)
            {
                Die();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        GolemController player;
        if (collider.gameObject.TryGetComponent<GolemController>(out player))
        {
            perpetuatedAttack = false;
        }
    }

    // Commit death
    void Die()
    {
        SceneManager.LoadScene("Credits");
    }
}
