using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadIfBelowYVal : MonoBehaviour
{
    [SerializeField] private float yThreshold = -20f;

    private void Update()
    {
        if (transform.position.y < yThreshold)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
