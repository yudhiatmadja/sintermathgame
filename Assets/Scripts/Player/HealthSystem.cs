using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 3; // Maksimal nyawa
    private int currentHealth;
    public Image[] hearts; // Array UI gambar hati
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public GameObject gameOverPanel;

    void Start()
    {
        currentHealth = maxHealth;
        gameOverPanel.SetActive(false);
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        // currentHealth -= damage;
        // if (currentHealth < 0) currentHealth = 0;
        // UpdateHearts();

        currentHealth -= damage;
        UpdateHearts();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        Debug.Log("Updating hearts...");
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
    void GameOver()
    {
        gameOverPanel.SetActive(true); // Tampilkan panel Game Over
        Time.timeScale = 0; // Pause game
    }

    
}
