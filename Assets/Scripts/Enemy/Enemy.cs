using UnityEngine;

public class Enemy : MonoBehaviour
{

    private LeaderboardManager leaderboardManager;

    void Start()
    {
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Bullet")){
            Destroy(gameObject);
            Destroy(other.gameObject);    
            LeaderboardManager leaderboardManager = FindObjectOfType<LeaderboardManager>();
        if (leaderboardManager != null)
        {
            leaderboardManager.SaveScore(100, "HighScoreMathQuiz");
        }
        }
    }
}
