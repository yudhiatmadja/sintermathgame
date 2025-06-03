using UnityEngine;
using UnityEngine.SceneManagement;

public class HouseTrigger : MonoBehaviour
{
    public string nextSceneName = "HouseQuiz";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Masuk trigger, langsung pindah scene");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}