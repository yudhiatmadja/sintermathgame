using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text characterNameText;
    public Text dialogText;
    public GameObject dialogPanel;
    public AudioSource audioSource;
    public AudioClip[] dialogAudioClips;

    private string[,] dialogs = {
        {"Santa", "Halo bang."},
        {"Denis", "Iya halo juga bang."},
        {"Santa", "Apa yang harus aku lakukan sekarang?"},
        {"Denis", "Jawab semua soal dan kalahkan musuh."},
        {"Santa", "Oke bang."}
    };

    private int currentDialogIndex = 0;

    void Start()
    {
        dialogPanel.SetActive(false); // Panel dialog tertutup di awal
    }

    void Update()
    {
        if (dialogPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            NextDialog();
        }
    }

    public void StartDialog()
    {
        if (!dialogPanel.activeSelf)
        {
            dialogPanel.SetActive(true);
            currentDialogIndex = 0;
            DisplayDialog();
        }
    }

    public void EndDialog()
    {
        if (dialogPanel.activeSelf)
        {
            dialogPanel.SetActive(false);
            characterNameText.text = "";
            dialogText.text = "";
        }
    }

    void DisplayDialog()
    {
        characterNameText.text = dialogs[currentDialogIndex, 0];
        dialogText.text = dialogs[currentDialogIndex, 1];
        PlayDialogAudio(currentDialogIndex);
    }

    void PlayDialogAudio(int index)
    {
        if (dialogAudioClips != null && index < dialogAudioClips.Length)
        {
            audioSource.clip = dialogAudioClips[index];
            audioSource.Play();
        }
    }

    void NextDialog()
{
    if (currentDialogIndex < dialogs.GetLength(0) - 1)
    {
        currentDialogIndex++;
        DisplayDialog();
    }
    else
    {
        EndDialog();
    }
}

    
}


