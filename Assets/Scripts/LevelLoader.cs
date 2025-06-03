using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void LoadLevel(int index)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(index);
    }
}
