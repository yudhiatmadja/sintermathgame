using UnityEngine;
using UnityEngine.SceneManagement;

public class BackScene : MonoBehaviour
{
       void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
