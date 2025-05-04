using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CreditsScroller : MonoBehaviour
{
    public float speed = 50f; 
    public RectTransform creditsText; 
    private float startY; 
    private float resetY; 

    void Start()
    {
        startY = creditsText.anchoredPosition.y; 
        resetY = startY - 1000f; 
    }

    void Update()
    {
        creditsText.anchoredPosition += Vector2.up * speed * Time.deltaTime;

        if (creditsText.anchoredPosition.y > resetY + 2280f)
        {
            creditsText.anchoredPosition = new Vector2(creditsText.anchoredPosition.x, startY);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
