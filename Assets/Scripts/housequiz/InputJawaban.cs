using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InputJawaban : MonoBehaviour
{
    public TMP_InputField inputField;
    public SpawnerSoal spawner;
    public int score;
    public TextMeshProUGUI scoreText;
    public GameObject finishPanel;

    void Start()
    {
        inputField.ActivateInputField();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string input = inputField.text;
            int playerAnswer;
            if (int.TryParse(input, out playerAnswer))
            {  
                foreach (var soal in FindObjectsOfType<SoalTextController>())
                {
                    if (soal.jawaban == playerAnswer)
                    {
                        soal.DestroyWithEffect(); 
                        score += 10;
                        scoreText.text = "Score: " + score;

                        if (score >= 200)
                        {
                            finishPanel.SetActive(true); 
                            Time.timeScale = 0;
                            inputField.interactable = false;
                        }

                        break; 
                    }

                }
            }

            inputField.text = "";
            inputField.ActivateInputField(); 
        }
    }

    private void ActivateInput()
    {
        EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
        inputField.ActivateInputField();
    }
}
