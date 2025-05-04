using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void PlayGame(){
        SceneManager.LoadScene("GamePlay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Credit()
    {
        SceneManager.LoadScene("CreditScene");
    }
    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }
}
