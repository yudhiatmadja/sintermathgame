using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    public GameObject finishPanel;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            finishPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

}
