using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int newScene = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCount  > newScene)
            {
                SceneManager.LoadScene(newScene);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
