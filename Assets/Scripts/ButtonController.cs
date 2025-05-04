


using TMPro;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public TMP_Text scoreText; // TextMeshPro untuk menampilkan skor
    private static int score = 0; // Gunakan static agar skor tidak reset antar soal
    public AudioClip benarSound;
    private AudioSource audioSource;
    private static int maxScore = 3;

    private void Awake()
    {
        score = 0; // Reset skor saat game dimulai ulang
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateScoreText(); // Pastikan skor awal ditampilkan
    }

    public void OnBenarClicked()
    {
        if (score >= maxScore)
        {
            return; // Cegah skor bertambah lebih dari 3
        }
        score++; // Tambah skor
        if (audioSource != null && benarSound != null)
        {
            audioSource.PlayOneShot(benarSound);
        }
        UpdateScoreText(); // Update tampilan skor
        PanelManager panelManager = Object.FindFirstObjectByType<PanelManager>();
        if (panelManager != null)
        {
            panelManager.MarkPanelAsAnswered(GetCurrentPanelName()); // Tandai panel sebagai dijawab benar
            panelManager.CloseAllPanels();
        }
    }

    public void OnSalahClicked()
    {
        CloseActivePanel(); // Tutup panel tanpa menambah skor
    }

    private void CloseActivePanel()
    {
        PanelManager panelManager = Object.FindFirstObjectByType<PanelManager>();

        if (panelManager != null)
        {
            panelManager.CloseAllPanels(); // Tutup semua panel
        }
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = score + "/3"; // Update teks skor
    }

    private string GetCurrentPanelName()
    {
        PanelManager panelManager = Object.FindFirstObjectByType<PanelManager>();
        if (panelManager != null)
        {
            foreach (var panel in panelManager.panels)
            {
                if (panel.activeSelf) 
                {
                    return panel.name;
                }
            }
        }
        return "";
    }
}

