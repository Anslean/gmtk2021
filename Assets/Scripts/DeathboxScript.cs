using UnityEngine;
using static Direction;

public class DeathboxScript : MonoBehaviour
{
    private bool died = false;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GolemController player;
        if (collider.gameObject.TryGetComponent<GolemController>(out player))
        {
            if (!player.isInvincibleToBoss())
            {
                // Player got kill'd by it!
                player.Die();
            }
        }
    }

    // Commit death
    void Die()
    {
        Destroy(gameObject);
    }
}
