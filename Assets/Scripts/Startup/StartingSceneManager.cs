using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartingSceneManager : MonoBehaviour
{
    public Text startingText; 
    public float letterDelay = 0.1f; 
    public string nextSceneName = "MainMenu"; 
    public float loopDuration = 5f; 

    private void Start()
    {
        StartCoroutine(TypeStartingTextLoop());
        StartCoroutine(LoadNextSceneAfterDelay(loopDuration));
    }

    private IEnumerator TypeStartingTextLoop()
    {
        string textToType = "Starting Game..."; 
        while (true) 
        {
            startingText.text = ""; 
            foreach (char letter in textToType)
            {
                startingText.text += letter; 
                yield return new WaitForSeconds(letterDelay); 
            }
            yield return new WaitForSeconds(0.5f); 
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); 
        SceneManager.LoadScene(nextSceneName); 
    }
}
