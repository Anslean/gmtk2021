using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTransition : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        GolemController player;
        if (collision.gameObject.TryGetComponent<GolemController>(out player))
        {
            SceneManager.LoadScene("BossTest");
        }
    }
}
