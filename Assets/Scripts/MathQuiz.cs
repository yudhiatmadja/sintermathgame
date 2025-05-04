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
    int level = Random.Range(1, 4); // Level 1 (kelas 1-2), Level 2 (kelas 3-4), Level 3 (kelas 5-6)
    int a, b;
    int operation = Random.Range(0, 4); // 0: +, 1: -, 2: ×, 3: ÷

    switch (level)
    {
        case 1: // Kelas 1-2 (penjumlahan & pengurangan sederhana)
            a = Random.Range(1, 20);
            b = Random.Range(1, 20);
            if (operation == 0) // Penjumlahan
            {
                correctAnswer = a + b;
                questionText.text = a + " + " + b + " = ?";
            }
            else // Pengurangan 
            {
                if (a < b) { int temp = a; a = b; b = temp; }
                correctAnswer = a - b;
                questionText.text = a + " - " + b + " = ?";
            }
            break;

        case 2: // Kelas 3-4 (perkalian & pembagian sederhana)
            a = Random.Range(2, 10);
            b = Random.Range(2, 10);
            if (operation < 2) // Gunakan penjumlahan atau pengurangan juga
            {
                correctAnswer = (operation == 0) ? a + b : a - b;
                questionText.text = a + (operation == 0 ? " + " : " - ") + b + " = ?";
            }
            else if (operation == 2) // Perkalian
            {
                correctAnswer = a * b;
                questionText.text = a + " × " + b + " = ?";
            }
            else // Pembagian 
            {
                correctAnswer = a;
                int hasil = a * b;
                questionText.text = hasil + " ÷ " + b + " = ?";
            }
            break;

        case 3: // Kelas 5-6 (operasi campuran dengan angka lebih besar)
            a = Random.Range(10, 100);
            b = Random.Range(2, 20);
            if (operation == 0) // Penjumlahan besar
            {
                correctAnswer = a + b;
                questionText.text = a + " + " + b + " = ?";
            }
            else if (operation == 1) // Pengurangan besar
            {
                if (a < b) { int temp = a; a = b; b = temp; }
                correctAnswer = a - b;
                questionText.text = a + " - " + b + " = ?";
            }
            else if (operation == 2) // Perkalian besar
            {
                correctAnswer = a * b;
                questionText.text = a + " × " + b + " = ?";
            }
            else // Pembagian besar 
            {
                correctAnswer = a;
                int hasil = a * b;
                questionText.text = hasil + " ÷ " + b + " = ?";
            }
            break;
    }
}


    public void SubmitAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer) && playerAnswer == correctAnswer)
        {
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
            Debug.Log("Jawaban salah!");
            healthSystem.TakeDamage(1);
            AudioManager.instance.PlaySound("hurt");
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
    }
}
