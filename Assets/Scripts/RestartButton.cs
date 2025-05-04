using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void RestartGame()
    {
        Time.timeScale = 1; // Normalkan waktu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Restart Scene
    }
}
