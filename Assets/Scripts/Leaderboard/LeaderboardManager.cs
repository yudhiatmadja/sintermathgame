using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class LeaderboardManager : MonoBehaviour
{
    public TextMeshProUGUI leaderboardNamesText;
    public TextMeshProUGUI leaderboardScoresText;

    private const string HighScoreKeyMathQuiz = "HighScoreMathQuiz";
    private const string HighScoreKeyHouseQuiz = "HighScoreHouseQuiz";

    private const int dummyPlayerCount = 5;
    private string[] firstNameParts = { "Crazy", "Brave", "Sneaky", "Smart", "Speedy", "Bold", "Furious", "Silent", "Wild", "Clever" };
    private string[] lastNameParts = { "Tiger", "Hawk", "Wolf", "Ninja", "Warrior", "Falcon", "Shadow", "Viper", "Hunter", "Dragon" };
    private string[] dummyNames;
    private int[] dummyScores;

    void Start()
    {
        dummyNames = new string[dummyPlayerCount];
        dummyScores = new int[dummyPlayerCount];
        UpdateLeaderboard();
    }

    public void SaveScore(int score, string quizType)
    {
        int currentScore = PlayerPrefs.GetInt(quizType, 0);
        int totalScore = currentScore + score;

        PlayerPrefs.SetInt(quizType, totalScore);
        PlayerPrefs.Save();

        UpdateLeaderboard();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            UpdateLeaderboard();
            int mathScore = PlayerPrefs.GetInt(HighScoreKeyMathQuiz, 0);
            int houseScore = PlayerPrefs.GetInt(HighScoreKeyHouseQuiz, 0);
            Debug.Log("Total Player Score: " + (mathScore + houseScore));
        }
    }

    private void UpdateLeaderboard()
    {
        int mathQuizScore = PlayerPrefs.GetInt(HighScoreKeyMathQuiz, 0);
        int houseQuizScore = PlayerPrefs.GetInt(HighScoreKeyHouseQuiz, 0);

        int totalScore = mathQuizScore + houseQuizScore;

        for (int i = 0; i < dummyPlayerCount; i++)
        {
            string randomFirstName = firstNameParts[Random.Range(0, firstNameParts.Length)];
            string randomLastName = lastNameParts[Random.Range(0, lastNameParts.Length)];
            dummyNames[i] = randomFirstName + " " + randomLastName;
            dummyScores[i] = Random.Range(1000, 20000);
        }

        var combined = dummyNames.Zip(dummyScores, (name, score) => new { name, score })
            .OrderByDescending(x => x.score)
            .ToList();

        string leaderboardNames = $"Me\n";
        string leaderboardScores = $"{totalScore}\n";

        foreach (var entry in combined)
        {
            leaderboardNames += $"{entry.name}\n";
            leaderboardScores += $"{entry.score}\n";
        }

        leaderboardNamesText.text = leaderboardNames;
        leaderboardScoresText.text = leaderboardScores;
    }

    
}
