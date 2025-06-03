using Unity.VisualScripting;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio Instance;
    public AudioSource audioSource;
    public AudioClip corectSound;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        audioSource.spatialBlend = 0f;
    }

    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "corect":
                audioSource.PlayOneShot(corectSound);
                break;        
        }
    }
}
