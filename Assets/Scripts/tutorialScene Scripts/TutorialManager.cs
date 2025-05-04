using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPages;
    public Button nextButton;
    public Button prevButton;

    private int currentPage = 0;

    void Start()
    {
        ShowPage(currentPage);
        
        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < tutorialPages.Length; i++)
        {
            tutorialPages[i].SetActive(i == index);
        }

        prevButton.gameObject.SetActive(index > 0);
        nextButton.gameObject.SetActive(index < tutorialPages.Length - 1);
    }

    public void NextPage()
    {
        if (currentPage < tutorialPages.Length - 1)
        {
            currentPage++;
            ShowPage(currentPage);
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            ShowPage(currentPage);
        }
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
