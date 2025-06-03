using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private LeaderboardManager leaderboardManager;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
        LeaderboardManager leaderboard = FindObjectOfType<LeaderboardManager>();
        if (leaderboardManager != null)
        {
            leaderboardManager.SaveScore(100, "HighScoreHouseQuiz");
        }
    }
}
