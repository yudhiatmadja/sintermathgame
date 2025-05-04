using UnityEngine;

public class OpenQuiz : MonoBehaviour
{
    public GameObject quizPanel;
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isOpen = !isOpen;  
            quizPanel.SetActive(isOpen);
        }
    }
}
