using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MathQuiz : MonoBehaviour
{
    public Text questionText;
    public TMP_InputField answerInput;
    public GameObject quizPanel;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public HealthSystem healthSystem;
    
    [Header("Level Settings")]
    public int level = 1; // Set ini di Inspector: 1 untuk scene level 1, 2 untuk scene level 2
    
    private int correctAnswer;
    private int randomCount = 0;
    private const int maxRandoms = 3;

    void Start()
    {
        GenerateQuestion();
        answerInput.onSubmit.AddListener(delegate { SubmitAnswer(); });
    }

    void GenerateQuestion()
    {
        int a, b;
        int operation = Random.Range(0, 4); // 0: +, 1: -, 2: ×, 3: ÷

        switch (level)
        {
            case 1: // Level 1 - Operasi campuran angka kecil (1-20)
                a = Random.Range(1, 21);
                b = Random.Range(1, 21);
                
                if (operation == 0) // Penjumlahan
                {
                    correctAnswer = a + b;
                    questionText.text = a + " + " + b + " = ?";
                }
                else if (operation == 1) // Pengurangan
                {
                    // Pastikan hasil tidak negatif
                    if (a < b) { int temp = a; a = b; b = temp; }
                    correctAnswer = a - b;
                    questionText.text = a + " - " + b + " = ?";
                }
                else if (operation == 2) // Perkalian
                {
                    // Gunakan angka lebih kecil untuk perkalian agar tidak terlalu sulit
                    a = Random.Range(1, 11);
                    b = Random.Range(1, 11);
                    correctAnswer = a * b;
                    questionText.text = a + " × " + b + " = ?";
                }
                else // Pembagian
                {
                    // Pastikan pembagian menghasilkan bilangan bulat
                    b = Random.Range(2, 11);
                    correctAnswer = Random.Range(1, 11);
                    a = correctAnswer * b;
                    questionText.text = a + " ÷ " + b + " = ?";
                }
                break;

            case 2: // Level 2 - Operasi campuran angka besar (10-100)
                a = Random.Range(10, 101);
                b = Random.Range(10, 51);
                
                if (operation == 0) // Penjumlahan
                {
                    correctAnswer = a + b;
                    questionText.text = a + " + " + b + " = ?";
                }
                else if (operation == 1) // Pengurangan
                {
                    // Pastikan hasil tidak negatif
                    if (a < b) { int temp = a; a = b; b = temp; }
                    correctAnswer = a - b;
                    questionText.text = a + " - " + b + " = ?";
                }
                else if (operation == 2) // Perkalian
                {
                    // Gunakan angka yang tidak terlalu besar untuk perkalian
                    a = Random.Range(10, 21);
                    b = Random.Range(2, 11);
                    correctAnswer = a * b;
                    questionText.text = a + " × " + b + " = ?";
                }
                else // Pembagian
                {
                    // Pastikan pembagian menghasilkan bilangan bulat
                    b = Random.Range(2, 21);
                    correctAnswer = Random.Range(5, 21);
                    a = correctAnswer * b;
                    questionText.text = a + " ÷ " + b + " = ?";
                }
                break;
                
            default:
                // Fallback ke level 1 jika level tidak valid
                level = 1;
                GenerateQuestion();
                return;
        }
    }

    public void SubmitAnswer()
    {
        Time.timeScale = 1;
        if (int.TryParse(answerInput.text, out int playerAnswer) && playerAnswer == correctAnswer)
        {
            Debug.Log("Jawaban benar!");
            ShootBullet();
            quizPanel.SetActive(false);
            answerInput.text = "";
            
            if (randomCount < maxRandoms)
            {
                randomCount++;
                GenerateQuestion();
            }
        }
        else
        {
            Debug.Log("Jawaban salah! Jawaban yang benar: " + correctAnswer);
            healthSystem.TakeDamage(1);
            AudioManager.instance.PlaySound("hurt");
            quizPanel.SetActive(false);
            answerInput.text = "";
        }
    }

    void ShootBullet()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        AudioManager.instance.PlaySound("shoot");
    }

    public void OpenQuizPanel()
    {
        quizPanel.SetActive(true);
        answerInput.Select();
        Time.timeScale = 0; // Pause game saat quiz dibuka
    }
    
    // Method untuk mendapatkan level saat ini
    public int GetCurrentLevel()
    {
        return level;
    }
}   